using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SnakeInstaller : MonoInstaller
{
    [SerializeField] private ObjectPrefabProvider prefabProvider;
    [SerializeField] private FactoryProvider factoryProvider;

    public override void InstallBindings()
    {
        Container.Bind<IObjectPrefabProvider>().FromInstance(prefabProvider).AsSingle();
        Container.Bind<IFactoryProvider>().FromInstance(factoryProvider).AsSingle();

        Container.BindFactory<Vector2, int, Snake, SnakeFactory>().FromFactory<SnakeFactory>();
        Container.BindFactory<Vector2, FoodFactory>().AsSingle();


        //Container.Bind<SnakeController>().AsSingle();
        //Container.Bind<ISnakeMovement>().To<KeyboardMover>().AsSingle();
        Container.BindInterfacesAndSelfTo<SnakeController>().AsSingle();
        Container.Bind<Food>().AsSingle();
        Container.Bind<FoodTemplate>().AsSingle();

        Container.Bind<SnakeManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<SnakeBuilder>().AsSingle();
        Container.Bind<SnakeCollisionHandler>().AsSingle();
        
        
    }
}