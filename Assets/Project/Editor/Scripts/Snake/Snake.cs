using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class Snake : MonoBehaviour
{
    private ISnakeCollisionHandler _collisionHandler;
    private SnakeController _controller;

    private ISnakeMovement _mover;

    private Transform _headTransform;
    private Transform _tailTransform;
    private List<Body> _bodyPositions;
    
    // public Snake(Transform headTransform, Transform tailTransform, List<Body> bodyPositions)
    // {
    //     _headTransform = headTransform;
    //     _tailTransform = tailTransform;
    //     _bodyPositions = bodyPositions;
    // }

    // public void Initialize(SnakeController controller, ISnakeMovement mover, Transform headTransform, Transform tailTransform, List<Body> bodyPositions)
    // {
    //     _controller = controller;
    //     _mover = mover;
    //     _mover.SetHeadTransform(headTransform);
    //     _mover.SetTailTransform(tailTransform);
    //     _mover.SetBodyPositions(bodyPositions);
    //     _controller.SetSnakeInstance(this);
    //     Debug.Log("Snake initialized with controller: " + controller);
    // }

    public void SetController(SnakeController controller)
    {
        _controller = controller; 
        // _headTransform = headTransform;
        // _tailTransform = tailTransform;
        // _bodyPositions = bodyPositions;
        //_mover = mover;
        // _mover.SetHeadTransform(headTransform);
        // _mover.SetTailTransform(tailTransform);
        // _mover.SetBodyParts(bodyPositions);
        Debug.Log("Controller set to: " + controller);
    }

    public ISnakeController GetController() => _controller;
    public ISnakeController GetSnakeInstance() => _controller;
    public void SetSnakeInstance(SnakeController snakeInstance) => _controller = snakeInstance;
    public void SetMover(KeyboardMover mover) => _mover = mover;
    public ISnakeMovement GetMover() => _mover;
    public List<Body> GetBodyPositions() => _bodyPositions;

    public Transform GetHeadTransform() => _headTransform;
    public Transform GetTailTransform() => _tailTransform;

    public void SetHeadTransform(Transform headTransform) => _headTransform = headTransform;
    public void SetTailTransform(Transform tailTransform) => _tailTransform = tailTransform;

    public void SetCollisionHandler(ISnakeCollisionHandler collisionHandler)
    {
        _collisionHandler = collisionHandler;
    }

    private void LateUpdate()
    {
        if (_collisionHandler != null)
        {
            _collisionHandler.MainHandler(gameObject);
        }
        else
            Debug.LogError("No collision handler");
    }

    private void Update()
    {
        if (_controller != null)
        {
            _controller.HandleMovement();
        }
        else
            Debug.LogError("No controller");
    }
}
