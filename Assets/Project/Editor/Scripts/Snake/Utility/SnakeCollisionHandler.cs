using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class SnakeCollisionHandler : MonoBehaviour, ISnakeCollisionHandler
{
    public event UnityAction SnakeCrash;
    private const float MAX_BOUNDS_X = 7f;
    private const float MAX_BOUNDS_Y = 5f;
    
    private List<Body> _bodyPositions;
    private Transform _headTransform;
    private Transform _tailTransform;

    public void SetDependencies(List<Body> bodyPositions, Transform headTransform, Transform tailTransform)
    {
        _bodyPositions = bodyPositions;
        _headTransform = headTransform;
        _tailTransform = tailTransform;
    }
    
    public void MainHandler(GameObject snakeTemplate)
    {
        try
        {
            CheckBounds(snakeTemplate);

            foreach (var body in _bodyPositions)
            {
                Collider2D bodyCollider = body.GetComponent<Collider2D>();
                if (_headTransform.GetComponent<Collider2D>().IsTouching(bodyCollider))
                    if (snakeTemplate != null && snakeTemplate.activeSelf)
                    {
                        SnakeCrash?.Invoke();
                        snakeTemplate.SetActive(false);
                        break;
                    }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception in SnakeCollisionHandler.MainHandler: " + e.Message);
            Debug.LogError("StackTrace: " + e.StackTrace);
        }
    }

    private void CheckBounds(GameObject snakeTemplate)
    {
        foreach (var bodyPart in _bodyPositions)
        {
            if (bodyPart.transform.position.x <= -MAX_BOUNDS_X || bodyPart.transform.position.x >= MAX_BOUNDS_X 
                                                               ||
                bodyPart.transform.position.y <= -MAX_BOUNDS_Y || bodyPart.transform.position.y >= MAX_BOUNDS_Y)
            {
                SnakeCrash?.Invoke();
                snakeTemplate.SetActive(false); // Отключить всю змею
                break;
            }
        }
    }

    public void InvokeSnakeCrash()
    {
        SnakeCrash?.Invoke();
    }
}
