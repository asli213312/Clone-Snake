using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using Zenject;

public class SnakeController : MonoBehaviour, ISnakeController, ISnakeInput, IFoodObserver, IFoodEatenHandler
{
   public event UnityAction<int> BodyAmountChanged = delegate { };
   public event Action<Vector2> DirectionChanged;
   private List<IFoodEatenHandler> _foodEatenHandlers = new List<IFoodEatenHandler>();

   private IFoodObserver _foodObserver;

   private ISnakeMovement _mover;

   [Inject] private SnakeFactory _snakeFactory;
   private IFactoryProvider _factoryProvider;

   private ISnakeInput _input;

   private readonly Dictionary<Vector2, float> _directionAngles;

   private Vector2 _currentDirection;
   private Vector2 _newDirection;

   private Transform _headTransform;
   private Transform _tailTransform;

   private List<Body> _bodyParts;

   private float _timeSinceLastMove;
   private float _currentMoveDelay;

   private bool _isDirectionSet;
   private bool _isBodyAmountChanged;
   public bool IsBodyAmountChanged { get; set; }

   public SnakeController()
   {
      _directionAngles = new Dictionary<Vector2, float>()
      {
         {Vector2.up, 0f},
         {Vector2.down, 180f},
         {Vector2.left, 90f},
         {Vector2.right, -90f}
      };
   }

   public void HeadCollision()
   {
      IncreaseSnakeByOne();
      OnFoodEaten();
   }

   public void OnFoodEaten()
   {
      foreach (var handler in _foodEatenHandlers)
      { 
         handler.OnFoodEaten();
      }
   }

   public void DecreaseSnakeByOne()
   {
      SnakeBuilder snakeBuilder = _snakeFactory.GetSnakeBuilder();
      ISnakeCollisionHandler collisionHandler = _snakeFactory.GetCollisionHandler();
      ISnakeMovement mover = _snakeFactory.GetMover();
      List<Body> bodies = snakeBuilder.GetBodyParts();
      if (snakeBuilder != null && bodies.Count > 2)
      {
         GameObject lastBodySegment = bodies[^2].gameObject;
         Vector2 lastBodyPosition = bodies[^2].GetPosition();

         snakeBuilder.RemoveBodySegment(lastBodySegment);
         bodies[^1].MoveTo(lastBodyPosition);

         Debug.Log("lastBodySegment is: " + lastBodySegment, lastBodySegment.gameObject);
      }
      if (bodies.Count <= 2)
      {
         collisionHandler.InvokeSnakeCrash();
         Debug.LogWarning("Cannot decrease snake size further.");
      }
      
      if (BodyAmountChanged != null)
      {
         BodyAmountChanged.Invoke(mover.GetBodyParts().Count - 2);
         Debug.Log("event bodyAmount is DECREASED");
      }

      Debug.Log("Snake is DECREASED by one!");
   }

   #region IncreaseSnakeByOne

   public void IncreaseSnakeByOne()
   {
      SnakeController controller = _snakeFactory.GetController();
      if (controller == null)
      { 
         Debug.LogError("SnakeController: controller is null"); 
         return;
      }
      Debug.Log("controller in SnakeController is found!");
      ISnakeMovement mover = _snakeFactory.GetMover();
      if (mover == null)
      { 
         Debug.LogError("SnakeController: mover is null"); 
         return;
      }
      Debug.Log("mover in SnakeController is found! " + controller.GetMover());
      Debug.Log("mover in SnakeController is found! " + mover);

      List<Body> bodies = mover.GetBodyParts();
      if (bodies == null)
      { 
         Debug.LogError("SnakeController: bodies is null"); 
         return;
      }
      Debug.Log("bodies in SnakeController is found! " + bodies);

      SnakeBuilder snakeBuilder = _snakeFactory.GetSnakeBuilder();
      if (snakeBuilder == null)
      { 
         Debug.LogError("SnakeController: snakeBuilder is null"); 
         return;
      }
      Debug.Log("snakeBuilder in SnakeController is found! " + snakeBuilder);

      Vector2 lastBodyPosition = bodies[^2].GetPosition();

      if (bodies.Count >= 2)
      {
         snakeBuilder.AddBodySegment(lastBodyPosition);   
      }

      int currentTailIndex = bodies.Count - 1;
      
      Debug.Log("tail: " + bodies[^2], bodies[^2].gameObject);
      Debug.Log("Body: " + bodies[^1], bodies[^1].gameObject);

      Vector2 newBodyPosition = CalculateNewBodyPosition(lastBodyPosition, GetCurrentDirection());
      
      //Debug.Break();
      (bodies[^2], bodies[currentTailIndex]) = (bodies[currentTailIndex], bodies[^2]);
      Debug.Log("newTail: " + bodies[currentTailIndex], bodies[currentTailIndex].gameObject);
      Debug.Log("newBody: " + bodies[^2], bodies[^2].gameObject);

      
      Vector2 tailPosition = bodies[^1].GetPosition();
      Vector2 tailNewPosition = tailPosition + (tailPosition - bodies[^2].GetPosition());
      
      // this alternative method (bugging)
      //Vector2 tailBufferPosition = tailPosition;
      //bodies[^2].MoveTo(tailBufferPosition);
      //bodies[^1].MoveTo(tailNewPosition);

      Debug.Log("tailNewPosition is calculated: " + tailNewPosition);
      
      if (BodyAmountChanged != null)
      {
         BodyAmountChanged.Invoke(mover.GetBodyParts().Count - 2);
      }
      Debug.Log("Snake is INCREASED by one!");
   }

   #endregion
   
   
   private Vector2 CalculateNewBodyPosition(Vector2 currentTailPosition, Vector2 currentDirection)
   {
      Vector2 newBodyPosition = currentTailPosition - currentDirection;
      return newBodyPosition;
   }

   public void HandleMovement()
   {
      CheckStartDirection();

      HandleInput();
      _timeSinceLastMove += Time.deltaTime;

      if (_timeSinceLastMove >= _factoryProvider.MoveDelay)
      {
         MoveSnake(_mover.GetCurrentDirection());
         _timeSinceLastMove = 0f;
      }
   }

   public void CheckStartDirection()
   {
      if (!_isDirectionSet)
      {
         if (_factoryProvider.IsVerticalPlacement)
            _currentDirection = Vector2.up;
         else
            _currentDirection = Vector2.left;

         _isDirectionSet = true;
      }
   }
   
   public void HandleInput()
   {
      #if UNITY_EDITOR
      if (Input.GetKeyDown(KeyCode.W))
         ChangeDirection(Vector2.up);
      
      if (Input.GetKeyDown(KeyCode.S))
         ChangeDirection(Vector2.down);

      if (Input.GetKeyDown(KeyCode.A))
         ChangeDirection(Vector2.left);
        
      if (Input.GetKeyDown(KeyCode.D))
         ChangeDirection(Vector2.right);
      #endif
   }
   
   public void ChangeDirection(Vector2 newDirection)
   {
      Vector2 currentDirection = _mover.GetCurrentDirection();
      if (currentDirection != newDirection)
      {
         Quaternion newAngle = Quaternion.Euler(0f, 0f, GetTargetAngle(newDirection));
         _mover.GetHeadTransform().rotation = newAngle;

         _currentDirection = newDirection;

         _mover.SetCurrentDirection(newDirection);
         DirectionChanged?.Invoke(newDirection);
      }
   }

   
   /// <summary>
   /// moving snake at current direction
   /// </summary>
   /// <param name="direction">direction for move snake</param>
   private void MoveSnake(Vector2 direction)
   {
      if (direction == Vector2.zero)
         direction = _currentDirection;

      List<Body> bodyPositions = _mover.GetBodyParts();

      for (int i = bodyPositions.Count - 1; i > 0; i--)
      {
         Vector2 currentPosition = bodyPositions[i - 1].GetPosition();

         bodyPositions[i].MoveTo(currentPosition);
      }
      
      Vector2 newHeadPosition = bodyPositions[0].GetPosition() + direction;
      Vector2 newTailPosition = bodyPositions[^1].GetPosition();
      
      bodyPositions[0].MoveTo(newHeadPosition);
      bodyPositions[^1].MoveTo(newTailPosition);



      //Debug.Log("rotation of tail: " + bodyPositions[^1].transform.localRotation);
      RotateTail();
   }

   private void RotateTail()
   {
      List<Body> bodies = _bodyParts;
      Vector3 currentTailPosition = bodies[^1].GetPosition();
      Vector3 nextTailPosition = bodies[^2].GetPosition(); // define next position

      Vector3 moveDirection = nextTailPosition - currentTailPosition;
      Quaternion newRotation = GetRotationForDirection(moveDirection);

      _mover.GetTailTransform().rotation = newRotation;


      //Debug.Log("currentDirection: " + rotation);
      //Debug.Log("currentTailPosition: " + currentTailPosition);
      //Debug.Log("nextTailPosition: " + nextTailPosition);
      //Debug.Log("moveDirection: " + moveDirection);
      //Debug.Break();
   }
   
   private Quaternion GetRotationForDirection(Vector3 direction)
   {
      if (_directionAngles.TryGetValue(direction, out float angle))
      {
         return Quaternion.Euler(0f, 0f, angle);
      }
      
      // if not found
      return Quaternion.identity;
   }

   public float GetTargetAngle(Vector2 newDirection)
   {
      if (_directionAngles.TryGetValue(newDirection, out float desiredAngle))
      {
         return desiredAngle;
      }
      return desiredAngle = 0f;
   }
   
   public void SetMover(ISnakeMovement mover, Transform headTransform, Transform tailTransform, List<Body> bodyParts)
   {
      _mover = mover;
      _mover.SetHeadTransform(headTransform);
      _mover.SetTailTransform(tailTransform);
      _mover.SetBodyParts(bodyParts);
      // _headTransform = headTransform;
      // _tailTransform = tailTransform;
      _bodyParts = bodyParts;
      _headTransform = headTransform;
      _tailTransform = tailTransform;
   }
   public Vector2 GetCurrentDirection() => _currentDirection;

   public float GetMoveDelay() => _currentMoveDelay;
   public void SetMoveDelay(float moveDelay) => _currentMoveDelay = moveDelay;
   
   public void SetPrefabFactoryProvider(IFactoryProvider provider)
   {
      _factoryProvider = provider;
   }

   public ISnakeMovement GetMover() => _mover;

   public IFoodObserver GetFoodObserver() => _foodObserver;
}

