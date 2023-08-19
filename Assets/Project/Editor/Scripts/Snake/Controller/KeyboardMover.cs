using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMover : MonoBehaviour, ISnakeMovement 
{
        public event Action<Body> BodyAdded;
        public event Action<Body> BodyRemoved;
        private Vector2 _currentDirection;
        private Transform _headTransform;
        private Transform _tailTransform;
        private Quaternion _lastBodyRotation;
        private List<Body> _bodyParts;
        private float _moveDelay;
        private ISnakeMovement _snakeMovement;
        private SnakeController _snakeController;

        // public KeyboardMover(SnakeController controller)
        // {
        //     _snakeController = controller;
        //     Debug.Log("KeyboardMover created with SnakeController: " + _snakeController);
        // }
        
        public float GetMoveDelay() => _snakeController.GetMoveDelay();
        public Vector2 GetCurrentDirection() => _snakeController.GetCurrentDirection();
        
        public Transform GetHeadTransform() => _headTransform;

        public Transform GetTailTransform() => _tailTransform;
        public List<Body> GetBodyParts() => _bodyParts;
        public void SetMoveDelay(float moveDelay) => _moveDelay = moveDelay;

        public void SetTailTransform(Transform tailTransform) => _tailTransform = tailTransform;

        public void SetHeadTransform(Transform headTransform) => _headTransform = headTransform;

        public void SetBodyParts(List<Body> bodyParts) => _bodyParts = bodyParts;

        public void SetCurrentDirection(Vector2 direction) => _currentDirection = direction;

        public void SetController(SnakeController controller) => _snakeController = controller;
}