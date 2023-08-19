using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class MobileInput : MonoBehaviour
{
    private SnakeController _snakeController;
    private bool _isInitialized;

    public void Initialize()
    {
        _snakeController = FindObjectOfType<SnakeController>();
        Debug.Log("Initialize in MobileInput is successful!");
            
        if (_snakeController != null)
        {
            Debug.Log("SnakeController in MobileInput is found!");
        }
        else
        {
            Debug.LogError("SnakeController in MobileInput is null!");
        }
    }

    private void Update()
    {
        if (_isInitialized == false && Time.time > 1f)
        {
            _isInitialized = true;
            Initialize();
        }
    }

    public void OnUpButtonPressed()
    {
        try
        {
            Debug.Log("Up button pressed!");
            if (_snakeController != null)
            {
                _snakeController.ChangeDirection(Vector2.up);
            }
            else
                Debug.LogError("controller in MobileInput is null!");    
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError("NullReferenceException occurred in MobileInput.OnUpButtonPressed: " + ex.Message);
            Debug.LogError("StackTrace: " + ex.StackTrace);
        }
    }

    public void OnDownButtonPressed()
    {
        _snakeController.ChangeDirection(Vector2.down);
    }

    public void OnLeftButtonPressed()
    {
        _snakeController.ChangeDirection(Vector2.left);
    }

    public void OnRightButtonPressed()
    {
        _snakeController.ChangeDirection(Vector2.right);
    }
}
