using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SnakeManager : MonoBehaviour
{
    [Inject] private SnakeFactory _snakeFactory;
    [SerializeField] [Range(1, 7)] private int bodyAmount = 3;
    [SerializeField] private bool verticalPlacement;
    [SerializeField] [Range(0.3f, 1f)] private float moveDelay = 1f;
    private bool _isInitialized;

    private void Start()
    {
        _snakeFactory.SetPlacement(verticalPlacement);
        _snakeFactory.SetDelay(moveDelay);
        
        Debug.Log("SnakeManager: Snake Creation...");

        Vector2 initialPositionVertical = new Vector2(0.5f, 3.5f);
        Vector2 initialPositionHorizontal = new Vector2(-1.5f, 0.5f);

        Snake snake; 
        
        if (verticalPlacement)
            snake = _snakeFactory.Create(initialPositionVertical, bodyAmount);
        else
            snake = _snakeFactory.Create(initialPositionHorizontal, bodyAmount);
        

        if (snake != null)
            Debug.Log("SnakeManager: Snake created successfully!");
        else
            Debug.LogError("SnakeManager: Snake creation failed.");
    }
}
