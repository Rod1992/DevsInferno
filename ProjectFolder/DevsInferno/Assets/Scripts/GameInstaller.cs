using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<CommandInvoker>().To<CommandInvokerGamePlay>().AsSingle().NonLazy();
        Container.Bind<MoveController>().AsSingle().NonLazy();
        Container.Bind<InputManager>().AsSingle().NonLazy();
    }

    
}
