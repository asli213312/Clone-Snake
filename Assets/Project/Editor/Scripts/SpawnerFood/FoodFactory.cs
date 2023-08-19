using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class FoodFactory : PlaceholderFactory<Vector2>
{
    private PoolFood _foodPool;
    private bool _spawnGoodFood;

    public Food CreateFood(Vector2 spawnPosition)
    {
        Food newFood = GetNextSuitableFoodFromPool(spawnPosition);

        if (newFood == null)
            return null;

        if (newFood.IsBadFood)
        {
            Debug.Log("Bad food", newFood.gameObject);
        }
        else if (newFood.IsBadFood == false)
        {
            _spawnGoodFood = true;
            Debug.Log("Good food", newFood.gameObject);
        }
        
        Debug.Log("spawnGoodFood: " + _spawnGoodFood);

        return newFood;
    }
    
    private Food GetNextSuitableFoodFromPool(Vector2 spawnPosition)
    {
        int totalFoodsChecked = 0;

        while (totalFoodsChecked < _foodPool.FoodList.Count)
        {
            Food currentFood = _foodPool.GetFood(spawnPosition);

            if (currentFood != null)
            {
                if (_spawnGoodFood && !currentFood.IsBadFood)
                {
                    Debug.Log("Found GOOD food, skipping...");
                    currentFood.gameObject.SetActive(false);
                    _foodPool.AddFoodToPool(currentFood);
                    totalFoodsChecked++;
                }
                else
                {
                    return currentFood;
                }
            }
            else
            {
                Debug.Log("No suitable food found in the pool");
                return null;
            }
            
            if (totalFoodsChecked >= _foodPool.FoodList.Count)
            {
                Debug.Log("All foods in the pool have been checked");
                break;
            }
        }

        return null;
    }

    public void ResetSpawnGoodFood()
    {
        _spawnGoodFood = false;
    }

    public PoolFood GetPool() => _foodPool;
    public void SetPoolFood(PoolFood foodPool) => _foodPool = foodPool;
}
