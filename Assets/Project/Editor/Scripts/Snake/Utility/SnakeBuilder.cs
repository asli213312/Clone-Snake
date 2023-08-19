using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SnakeBuilder : MonoBehaviour
{
    [Inject] private IObjectPrefabProvider _prefabProvider;
    [Inject] private IFactoryProvider _factoryProvider;

    private Snake _snakeTemplate;
    
    private Transform _headTransform;
    private Transform _tailTransform;

    private List<Body> _bodyParts = new();

    private Vector2 _tailOffset; // required offset
    
    public Snake CreateSnake(Vector2 initialHeadPosition, int bodyAmount)
    {
        Debug.Log("SnakeBuilder: Creating snake...");
        
        _snakeTemplate = new GameObject("Snake").AddComponent<Snake>();

        BuildSnake(initialHeadPosition, bodyAmount);

        return _snakeTemplate;
    }

    private void BuildSnake(Vector2 initialHeadPosition, int bodyAmount)
    {
        _bodyParts.Clear();

        AddHeadSegment(initialHeadPosition);
        BuildBody(initialHeadPosition, bodyAmount);
        AddTailSegment();
    }

    private void BuildBody(Vector2 initialPosition, int bodyAmount)
    {
        Vector2 bodyPosition = new Vector2(initialPosition.x, initialPosition.y);
        for (int i = 0; i < bodyAmount; i++)
        {
            if (_factoryProvider.IsVerticalPlacement)
            {
                bodyPosition += new Vector2(0, -1f); // Offset the body position
            }
            else
            {
                bodyPosition += new Vector2(1f, 0); // Offset the body position
            }

            AddBodySegment(bodyPosition);
        }
    }

    private void AddHeadSegment(Vector2 initialPosition)
    {
        GameObject newHeadObject = AddSnakeSegment(initialPosition, _prefabProvider.HeadPrefab, true, false);
        newHeadObject.name = "Head";
        _headTransform = newHeadObject.transform;
    }

    public void RemoveBodySegment(GameObject bodySegment)
    {
        int index = _bodyParts.FindIndex(segment => segment.gameObject == bodySegment);

        if (index >= 0)
        {
            Destroy(bodySegment);
            
            _bodyParts.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Body segment not found for removal.");
        }
    }

    public void AddBodySegment(Vector2 initialPosition)
    {
        AddSnakeSegment(initialPosition, _prefabProvider.BodyPrefab, false, false);
    }

    private void AddTailSegment()
    {
        Vector2 lastBodyPosition = _bodyParts[^1].GetPosition();
        Vector2 tailOffset = new Vector2(0, -1.085f);
        if (!_factoryProvider.IsVerticalPlacement)
            tailOffset = new Vector2(1.085f, 0);        
        
        
        Vector2 tailPosition = lastBodyPosition + tailOffset;
        //Debug.Log("Last body position: " + lastBodyPosition);
        
        AddSnakeSegment(tailPosition, _prefabProvider.TailPrefab, false, true);
    }
    
    private GameObject AddSnakeSegment(Vector2 initialPosition, GameObject prefab, bool isHeadSegment, bool isTailSegment)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }
        //Debug.Log("Creating snake segment at position: " + initialPosition + prefab.name);
        float initPositionX = initialPosition.x;
        float initPositionY = initialPosition.y;
        GameObject newObject = Instantiate(prefab, new Vector3(initPositionX, initPositionY, 0f), Quaternion.identity);
        newObject.transform.SetParent(_snakeTemplate.transform);

        if (newObject == null)
        {
            Debug.LogError("Cannot create object using prefab " + prefab.name);
        }

        Body newBody = newObject.GetComponent<Body>();
        if (newBody == null)
        {
            Debug.LogError("Object is created from prefab " + prefab.name + ", but not contain Body component.");
        }

        if (isHeadSegment)
            _headTransform = newObject.transform;

        if (isTailSegment)
            _tailTransform = newObject.transform;
        
        newBody.MoveTo(initialPosition);
        if (!_factoryProvider.IsVerticalPlacement)
            newBody.RotateTo(Quaternion.Euler(0,0,90));
        
        _bodyParts.Add(newBody);
        return newObject;
    }
    
    public List<Body> GetBodyParts() => _bodyParts;

    public bool GetIsVerticalPlacement() => _factoryProvider.IsVerticalPlacement;
    
    public Transform GetHeadTransform() => _headTransform;
    public Transform GetTailTransform() => _tailTransform;
    public void SetTailTransform(Transform tailTransform)
    {
        _tailTransform = tailTransform;
    }
}
