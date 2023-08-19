using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FoodCollisionHandler : MonoBehaviour
{
    private SnakeBuilder _snakeBuilder;
    private SnakeController _snakeController;
    private Transform _headTransform;

    public void Initialize(SnakeBuilder snakeBuilder, Transform headTransform)
    {
        _snakeBuilder = snakeBuilder;
        _headTransform = headTransform;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Transform headTransform = _snakeBuilder.GetHeadTransform();
        /*if (headTransform != null)
        {
            Debug.Log("head is HAS");
            if (col.gameObject.TryGetComponent(out Food food))
            {
                List<Body> bodies = _snakeController.GetMover().GetBodyParts();
                Debug.Log("first Body: " + bodies[0].gameObject, gameObject);    
            }
            // ...
        }
        else
            Debug.Log("head is null");*/
        // if (col.TryGetComponent<Food>(out var food))
        // {
        //     Vector3 newPosition = bodies[^2].GetPosition() - bodies[^1].GetPosition();
        //     GameObject newBodyObject = Instantiate(bodies[1].gameObject, newPosition, Quaternion.identity);
        //     Body newBodyComponent = newBodyObject.GetComponent<Body>();
        //
        //     newBodyComponent.CopyFrom(bodies[1]);
        //
        //     food.gameObject.SetActive(false);
        // }
    }
}
