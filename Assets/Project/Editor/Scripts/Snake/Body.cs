using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    private Vector2 _position;
    private Quaternion _rotation;
    private Vector2 _lastBodyDirection = Vector2.up;
    
    private Quaternion _tailRotation = Quaternion.identity;

    public Body(Vector2 initialPosition, Quaternion initialRotation)
    {
        _position = initialPosition;
        _rotation = initialRotation;
    }

    public void MoveTo(Vector2 newPosition)
    {
        _lastBodyDirection = newPosition - _position;
        _position = newPosition;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
    
    public void RotateTo(Quaternion newRotation)
    {
        _rotation = newRotation;
        transform.rotation = newRotation;
    }
    
    public void CopyFrom(Body source)
    {
        // Копируем спрайт (Renderer) и другие компоненты
        SpriteRenderer sourceRenderer = source.GetComponent<SpriteRenderer>();
        if (sourceRenderer != null)
        {
            SpriteRenderer newRenderer = gameObject.AddComponent<SpriteRenderer>();
            newRenderer.sprite = sourceRenderer.sprite;
            newRenderer.color = sourceRenderer.color;
            // Копируйте другие параметры спрайта по аналогии
        }

        // Копируем другие компоненты и параметры Body
        // ...
    }

    public Vector2 GetPosition() => _position;
    public Quaternion GetRotation() => _rotation;
    public void SetPosition(Vector2 position) => _position = position;
    public void SetRotation(Quaternion rotation) => _rotation = rotation;
}
