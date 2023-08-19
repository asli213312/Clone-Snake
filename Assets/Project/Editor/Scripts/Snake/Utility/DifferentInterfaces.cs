using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISnakeBuilder
{
    Snake CreateSnake(Vector2 initialHeadPosition, int bodyAmount);
    public List<Body> GetBodyParts();
    public Transform GetHeadTransform();
    public Transform GetTailTransform();
}

public interface ISnakeCollisionHandler
{
    event UnityAction SnakeCrash;
    void SetDependencies(List<Body> bodyParts, Transform headTransform, Transform tailTransform);
    void MainHandler(GameObject snakeTemplate);
    void InvokeSnakeCrash();
}

public interface ISnakeController
{
    void HandleMovement();
    void ChangeDirection(Vector2 newDirection);
    void SetMover(ISnakeMovement mover, Transform headTransform, Transform tailTransform, List<Body> bodyParts);
}

public interface IScoreObserver
{
    void HandleFoodScoreChanged(int newScore);
}

public interface IFactory
{
    Snake Create(Vector2 startingPosition, int bodyAmount);
}
