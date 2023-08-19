using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour, ISpawner
{
    [SerializeField] private FoodTemplate goodFoodTemplate;
    [SerializeField] private FoodTemplate badFoodTemplate;
    //[SerializeField] private int maxFoodCount;
    [SerializeField] private float spawnInterval;
    [SerializeField] private bool badIsCanIncMoveDelay;
    [SerializeField] private PoolFood poolFood;

    private float _timeToSpawn;
    private const int MAX_POSITION_X = 6;
    private const int MAX_POSITION_Y = 4;

    private Food _currentGoodFood;
    private int _foodCount;
    private const float FOOD_COLLIDER_RADIUS = 0.5f;
    private const int FOOD_POOL_SIZE = 50;

    [Inject] private SnakeFactory _snakeFactory;
    [Inject] private FoodFactory _foodFactory;

    private void Start()
    {
        try
        {
            poolFood.InitializePool(goodFoodTemplate, badFoodTemplate, FOOD_POOL_SIZE);
            _foodFactory.SetPoolFood(poolFood);
        }
        catch (NullReferenceException ex)
        {
            Debug.LogError("NullReferenceException occurred in Spawner.Start: " + ex.Message);
            Debug.LogError("StackTrace: " + ex.StackTrace);
        }
    }

    private void Update()
    {
        _timeToSpawn += Time.deltaTime;
        if (_timeToSpawn >= spawnInterval)
        {
            CheckGoodFoodIsEaten();
            SpawnFood();
            _timeToSpawn = 0;
        }
    }

    public void SpawnFood()
    {
        SnakeController snakeController = _snakeFactory.GetController();
        IFactoryProvider factoryProvider = _snakeFactory.GetFactoryProvider();

        Vector2 spawnPosition = GetRandomPosition();

        // Create food with dependencies
        Food newFood = _foodFactory.CreateFood(spawnPosition);
        if (newFood != null)
        {
            newFood.Initialize(snakeController, factoryProvider, poolFood);

            if (newFood.IsBadFood == false)
            {
                _currentGoodFood = newFood;
                Debug.Log("is current GOOD food: " + _currentGoodFood, _currentGoodFood.gameObject);
            }
        }
        else
        {
            Debug.Log("No suitable food available to spawn");
        }
    }

    private Vector2 GetRandomPosition()
    {
        int randomX = Random.Range(-MAX_POSITION_X, MAX_POSITION_X);
        int randomY = Random.Range(-MAX_POSITION_Y, MAX_POSITION_Y);
        float offsetPosition = 0.5f;
        
        float offsetX = Random.Range(0, 1) == 0 ? -offsetPosition : offsetPosition;
        float offsetY = Random.Range(0, 1) == 0 ? -offsetPosition : offsetPosition;
        
        Vector2 spawnPosition = new Vector2(randomX + offsetX, randomY + offsetY);
        
        if (IsFoodOverlap(spawnPosition))
            return GetRandomPosition();

        return spawnPosition;
    }
    
    private bool IsFoodOverlap(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, FOOD_COLLIDER_RADIUS);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Food>() != null)
            {
                Debug.Log("Food detected at the same position as new spawn!");
                Debug.Log("Detected Food Object: " + collider.gameObject.name, collider.gameObject);
                return true;
            }
            if (collider.GetComponent<Body>() != null)
            {
                Debug.Log("Food detected at the body position!");
                Debug.Log("Detected Food Object: " + collider.gameObject.name, collider.gameObject);
                //Debug.Break();
                return true;
            }
        }

        return false;
    }

    private void CheckGoodFoodIsEaten()
    {
        if (_currentGoodFood != null)
            if (!_currentGoodFood.gameObject.activeSelf)
            {
                _foodFactory.ResetSpawnGoodFood();
                Debug.Log("Spawner:  Reset Spawn Good Food");
            }
    }
}
