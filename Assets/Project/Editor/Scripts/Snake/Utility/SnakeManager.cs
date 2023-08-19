using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SnakeManager : MonoBehaviour
{
    [Inject] private SnakeFactory _snakeFactory;
    [SerializeField] [Range(1, 10)] private int bodyAmount = 3;
    [SerializeField] private bool verticalPlacement;
    [SerializeField] [Range(0.3f, 1f)] private float moveDelay = 1f;

    private void Start()
    {
        _snakeFactory.SetPlacement(verticalPlacement);
        _snakeFactory.SetDelay(moveDelay);

        Debug.Log("SnakeCreationTester: Testing Snake Creation...");

        Vector2 initialPosition = new Vector2(0.5f, 0.5f);

        Snake snake = _snakeFactory.Create(initialPosition, bodyAmount);

        if (snake != null)
            Debug.Log("SnakeCreationTester: Snake created successfully!");
        else
            Debug.LogError("SnakeCreationTester: Snake creation failed.");
    }
}
