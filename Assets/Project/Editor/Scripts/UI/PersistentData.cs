using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentData", menuName = "Game/Data")]
public class PersistentData : ScriptableObject
{
    public int highScore;
}
