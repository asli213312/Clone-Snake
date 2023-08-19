using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodFood : MonoBehaviour
{
    private Food _foodComponent;

    private void Awake()
    {
        _foodComponent = GetComponent<Food>();
    }

    private void OnFoodEaten()
    {
        Debug.Log("IS GOOD FOOD");
        _foodComponent.IsBadFood = false;
        _foodComponent.SnakeController.IncreaseSnakeByOne();
        _foodComponent.MainFoodHandler();
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Head"))
        {
            Debug.Log("Head collided with food!");
            OnFoodEaten();
        }
    }
}
