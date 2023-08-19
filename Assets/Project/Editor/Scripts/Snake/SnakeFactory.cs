using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class SnakeFactory : PlaceholderFactory<Vector2, int, Snake>, IFactory
{
    private readonly DiContainer _container;
    private readonly IFactoryProvider _prefabProvider;
    private SnakeController _snakeController;
    private ISnakeCollisionHandler _snakeCollisionHandler;
    private ISnakeMovement _mover;
    private SnakeBuilder _snakeBuilder;

    public SnakeFactory(DiContainer container, IFactoryProvider factoryProvider)
    {
        _container = container;
        _prefabProvider = factoryProvider;
    }

    public Snake Create(Vector2 startPosition, int bodyAmount)
    {
        Debug.Log("SnakeFactory: Creating snake...");

        Snake snake = CreateSnakeWithoutInjection(startPosition, bodyAmount);

        Debug.Log("SnakeFactory: Snake created.");

        return snake; 
    }
    
    private ISnakeCollisionHandler CreateCollisionHandler(Transform headTransform, Transform tailTransform, List<Body> bodyPositions)
    {
        // Создаем экземпляр SnakeCollisionHandler и внедряем зависимости
        ISnakeCollisionHandler collisionHandler = new SnakeCollisionHandler();
        collisionHandler.SetDependencies(bodyPositions, headTransform, tailTransform);

        return collisionHandler;
    }

    private Snake CreateSnakeWithoutInjection(Vector2 startPosition, int bodyAmount)
    {
        SnakeBuilder snakeBuilderPrefab = _prefabProvider.SnakeBuilder;
        _snakeBuilder = _container.InstantiatePrefabForComponent<SnakeBuilder>(snakeBuilderPrefab);
        SnakeController snakeControllerPrefab = _prefabProvider.SnakeController;
        SnakeController snakeController = _container.InstantiatePrefabForComponent<SnakeController>(snakeControllerPrefab);
        KeyboardMover keyboardMoverPrefab = _prefabProvider.KeyboardMover;
        _mover = _container.InstantiatePrefabForComponent<KeyboardMover>(keyboardMoverPrefab);

        _snakeController = snakeController;

        Snake snakeTemplate = _snakeBuilder.CreateSnake(startPosition, bodyAmount);
        snakeTemplate.gameObject.AddComponent<SnakeCollisionHandler>();
        snakeController = snakeTemplate.gameObject.AddComponent<SnakeController>();

        Transform headTransform = _snakeBuilder.GetHeadTransform();
        Transform tailTransform = _snakeBuilder.GetTailTransform();
        List<Body> bodyParts = _snakeBuilder.GetBodyParts();

        _snakeCollisionHandler = CreateCollisionHandler(headTransform, tailTransform, bodyParts);
        _snakeCollisionHandler.SetDependencies(bodyParts, headTransform, tailTransform);

        _mover.SetController(snakeController);
        snakeController.SetMover(_mover, headTransform, tailTransform, bodyParts);

        snakeTemplate.SetCollisionHandler(_snakeCollisionHandler);
        snakeTemplate.SetController(snakeController);

        snakeController.SetPrefabFactoryProvider(GetFactoryProvider());

        return snakeTemplate;
    }

    public SnakeBuilder GetSnakeBuilder() => _snakeBuilder;
    public IFactoryProvider GetFactoryProvider() => _prefabProvider;
    public SnakeController GetController() => _snakeController;
    public ISnakeMovement GetMover() => _mover;
    public ISnakeCollisionHandler GetCollisionHandler() => _snakeCollisionHandler;

    public void SetPlacement(bool isVerticalPlacement)
    {
        _prefabProvider.IsVerticalPlacement = isVerticalPlacement;
    }

    public void SetDelay(float moveDelay)
    {
        _prefabProvider.MoveDelay = moveDelay;
    }
}

public class ComponentProvider : IComponentProvider
{
    private readonly DiContainer _container; 
    //private readonly IFactoryProvider _prefabProvider;

    /*public ComponentProvider(DiContainer container)
    {
        _container = container;
    }

    public SnakeBuilder CreateSnakeBuilder()
    {
        return _container.InstantiatePrefabForComponent<SnakeBuilder>(_prefabProvider.SnakeBuilder);
    }

    public SnakeController CreateSnakeController()
    {
        return _container.InstantiatePrefabForComponent<SnakeController>(_prefabProvider.SnakeController);
    }

    public KeyboardMover CreateKeyboardMover()
    {
        return _container.InstantiatePrefabForComponent<KeyboardMover>(_prefabProvider.KeyboardMover);
    }*/
}

public interface IComponentProvider
{
    /*SnakeBuilder CreateSnakeBuilder();
    SnakeController CreateSnakeController();
    KeyboardMover CreateKeyboardMover(); */   
}



