using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class BodyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    private int _counter;

    [Inject] private SnakeFactory _snakeFactory;
    private SnakeController _snakeController;
    private ISnakeMovement _moverSnake;

    private bool _isInitialized;

    private void Initialize()
    {
        if (_isInitialized)
        {
            _snakeController = _snakeFactory.GetController();
            Debug.Log("controller in BodyCounter is Found!");
            _moverSnake = _snakeFactory.GetMover();
            Debug.Log("mover in BodyCounter is Found!");

            if (_snakeController != null && _moverSnake != null)
            {
                _snakeController.BodyAmountChanged += UpdateCounterText;
                Debug.Log("Event in BodyCounter is Subscribed!");
            }
            else
            {
                Debug.LogError("Something is Null in BodyCounter");
            }  
            UpdateCounterText(_moverSnake.GetBodyParts().Count - 2);
        }
        
        Debug.Log("Initialize in BodyCounter is successful!");
    }

    private void Update()
    {
        if (_isInitialized == false)
        {
            _isInitialized = true; 
            Initialize();
        }
    }

    private void OnDisable()
    {
        if (_snakeController != null)
        {
            _snakeController.BodyAmountChanged -= UpdateCounterText;
            Debug.Log("Event in BodyCounter is UNsubscribed!");
        }
    }

    private void UpdateCounterText(int bodyCount)
    {
        counterText.text = bodyCount.ToString();
    }
}
