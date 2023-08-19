using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryProvider : MonoBehaviour, IFactoryProvider
{
    public SnakeBuilder snakeBuilderPrefab;
    public SnakeController snakeControllerPrefab;
    public KeyboardMover keyboardMoverPrefab;

    public SnakeBuilder SnakeBuilder => snakeBuilderPrefab;
    public SnakeController SnakeController => snakeControllerPrefab;
    public KeyboardMover KeyboardMover => keyboardMoverPrefab;
    public bool IsVerticalPlacement { get; set; }
    public float MoveDelay { get; set; }
}

public interface IFactoryProvider 
{
    SnakeBuilder SnakeBuilder { get; }
    SnakeController SnakeController { get; }
    KeyboardMover KeyboardMover { get; }
    public bool IsVerticalPlacement { get; set; }
    public float MoveDelay { get; set; }
}
