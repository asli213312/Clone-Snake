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

    public void SetController(SnakeController controller)
    {
        _controller = controller;
        Debug.Log("Controller set to: " + controller);
    }

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
