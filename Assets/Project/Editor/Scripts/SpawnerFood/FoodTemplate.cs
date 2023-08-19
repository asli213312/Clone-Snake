using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodTemplate", menuName = "Custom/Food Template")]
public class FoodTemplate : ScriptableObject
{
    public List<Food> foodList;
    public bool IsBadFood { get; set; }
}
