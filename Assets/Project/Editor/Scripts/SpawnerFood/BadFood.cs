using System.Collections;
using System.Collections.Generic;
using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using UnityEngine;

public class BadFood : MonoBehaviour
{
    private Food _foodComponent; // Ссылка на родительский компонент Food

    private void Awake()
    {
        _foodComponent = GetComponent<Food>(); // Получаем ссылку на родительский компонент
    }

    private void OnFoodEaten()
    {
        Debug.Log("IS BAD FOOD");
        _foodComponent.SnakeController.DecreaseSnakeByOne();
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
