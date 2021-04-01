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
    public MoveController moveController;
    public InputManager inputManager;
    public CameraController cameraController;
    public ParticlesManager particlesManager;


    public static Game Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public void Constructor(ICommandInvoker invoker, MoveController _moveController, InputManager _inputManager, CameraController _cameraController, ParticlesManager _particlesManager)
    {
        instance = this;
        commandInvoker = invoker;
        moveController = _moveController;
        inputManager = _inputManager;
        cameraController = _cameraController;
        particlesManager = _particlesManager;

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
