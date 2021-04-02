using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{
    static Game instance;
    private ICommandInvoker commandInvoker;
    public MoveController MoveControl
    {
        get;
        private set;
    }
    public InputManager Inputs
    {
        get;
        private set;
    }
    public CameraController Camera
    {
        get;
        private set;
    }
    public ParticlesManager Particles
    {
        get;
        private set;
    }
    public PlayerController Player
    {
        get;
        private set;
    }


    public static Game Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public void Constructor(ICommandInvoker invoker, MoveController _moveController, InputManager _inputManager, CameraController _cameraController, ParticlesManager _particlesManager, PlayerController playerController)
    {
        instance = this;
        commandInvoker = invoker;
        MoveControl = _moveController;
        Inputs = _inputManager;
        Camera = _cameraController;
        Particles = _particlesManager;
        Player = playerController;

        Observable.EveryFixedUpdate().Subscribe(x => { commandInvoker.ExecuteCommands(); });
    }

    public void AddCommand(ICommand command)
    {
        if(!commandInvoker.IsUndoing())
        commandInvoker.Add(command);
    }

    public void UndoAll()
    {
        commandInvoker.UndoUntilTimestamp(0, Time.time);
    }

    public void OnDestroy()
    {
        commandInvoker.Dispose();
    }
}
