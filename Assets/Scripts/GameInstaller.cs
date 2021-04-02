using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    ParticlesManager prefabParticlesManager = null;
    [SerializeField]
    PlayerController prefabPlayerController = null;

    public override void InstallBindings()
    {
        Container.Bind<ICommandInvoker>().To<CommandInvokerGamePlay>().AsSingle().NonLazy();
        Container.Bind<EventBus>().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInNewPrefab(prefabPlayerController).AsSingle().NonLazy();
        Container.Bind<CameraController>().AsSingle().NonLazy();
        Container.Bind<MoveController>().AsSingle().NonLazy();
        Container.Bind<InputManager>().AsSingle().NonLazy();
        Container.Bind<ParticlesManager>().FromComponentsInNewPrefab(prefabParticlesManager).AsSingle().NonLazy();
    }

    
}
