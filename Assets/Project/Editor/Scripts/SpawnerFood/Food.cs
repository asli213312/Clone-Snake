using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using Zenject;

public class Food : MonoBehaviour
{
    [SerializeField] private int foodScore;
    public event UnityAction<int> ScoreHasChanged;
    public SnakeController SnakeController { get; set; }
    private IFactoryProvider _factoryProvider;
    private PoolFood _poolFood;
    public bool IsBadFood { get; set; }

    public void Initialize(SnakeController snakeController, IFactoryProvider factoryProvider, PoolFood poolFood)
    {
        SnakeController = snakeController;
        _factoryProvider = factoryProvider;
        _poolFood = poolFood;
    }

    public void MainFoodHandler()
    {
        if (SnakeController == null)
            Debug.LogError("SnakeController in Food is NULL");
        else
            Debug.Log("SnakeController in Food is FOUND");
        
        if (_factoryProvider == null)
            Debug.LogError("FactoryProvider in Food is NULL");
        else
            Debug.Log("FactoryProvider in Food is FOUND");
        
        float newDelay = 0.005f;
        if (!IsBadFood)
            _factoryProvider.MoveDelay -= newDelay;
        else
            _factoryProvider.MoveDelay -= newDelay;
        
        Debug.Log("delay in food: " + _factoryProvider.MoveDelay);
        
        //Debug.Log("delay in food: " + _factoryProvider.MoveDelay);
        
        //Debug.Break();
        
        ScoreHasChanged?.Invoke(foodScore);
        
        _poolFood.HandleFoodScoreChanged(foodScore);

        _poolFood.AddFoodToPool(this);
    }

    public void SetSnakeController(SnakeController snakeController) => SnakeController = snakeController;
    public void SetFactoryProvider(IFactoryProvider provider) => _factoryProvider = provider;
}
