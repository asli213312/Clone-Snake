using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PoolFood : MonoBehaviour
{
    public Queue<Food> FoodList = new Queue<Food>();
    public event UnityAction<int> FoodScoreChanged;
    private ScoreAndHighScore _scoreManager;
    private int _poolSize;
    public bool IsBadFood {get; set; }

    public void InitializePool(FoodTemplate goodFoodTemplate, FoodTemplate badFoodTemplate, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Food newFood = InstantiateFood(goodFoodTemplate, badFoodTemplate, Vector3.zero);
            AddFoodToPool(newFood);
        }
    }

    private Food InstantiateFood(FoodTemplate goodFoodTemplate, FoodTemplate badFoodTemplate, Vector3 position)
    {
        float randomValue = Random.Range(0f, 1f);
        IsBadFood = randomValue <= 0.7f; // 60% вероятность выбора плохой еды

        List<Food> selectedFoodPrefabs = IsBadFood ? badFoodTemplate.foodList : goodFoodTemplate.foodList;

        int randomFoodIndex = Random.Range(0, selectedFoodPrefabs.Count);
        Food selectedFoodPrefab = selectedFoodPrefabs[randomFoodIndex];
        
        Food food = Instantiate(selectedFoodPrefab, position, Quaternion.identity);
        food.IsBadFood = IsBadFood;
        food.gameObject.SetActive(false);
        food.transform.parent = transform;

        if (food.IsBadFood)
            food.AddComponent<BadFood>();
        else
            food.AddComponent<GoodFood>();

        return food;
    }

    public void AddFoodToPool(Food food)
    {
        food.gameObject.SetActive(false);
        FoodList.Enqueue(food);
    }

    public Food GetFood(Vector2 position)
    {
        if (FoodList.Count == 0)
            return null;
        
        Food newFood = FoodList.Dequeue();
        newFood.transform.position = position;
        newFood.gameObject.SetActive(true);
        return newFood;
    }
    
    public void HandleFoodScoreChanged(int foodScore)
    {
        FoodScoreChanged?.Invoke(foodScore);
    }
}
