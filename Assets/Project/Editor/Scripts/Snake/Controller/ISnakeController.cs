using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISnakeInput
{
    event Action<Vector2> DirectionChanged;
    void HandleInput();
}

public interface ISnakeMovement
{
    float GetMoveDelay();
    Vector2 GetCurrentDirection();
    List<Body> GetBodyParts();
    Transform GetHeadTransform();
    Transform GetTailTransform();
    void SetMoveDelay(float moveDelay);
    void SetTailTransform(Transform tailTransform);
    void SetHeadTransform(Transform headTransform);
    void SetBodyParts(List<Body> bodyParts);
    void SetCurrentDirection(Vector2 direction);
    void SetController(SnakeController controller);
}

public interface IFoodObserver
{
    void HeadCollision();
}
public interface IFoodEatenHandler
{
    void OnFoodEaten();
}

public interface ISpawner
{
    void SpawnFood();
}