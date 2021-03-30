using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class Game : MonoBehaviour
{
    static Game instance;
    private ICommandInvoker commandInvoker;
    public MoveController moveController;
    public InputManager inputManager;
    public CameraController cameraController;

    public static Game Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public void Constructor(ICommandInvoker invoker, MoveController _moveController, InputManager _inputManager, CameraController _cameraController)
    {
        instance = this;
        commandInvoker = invoker;
        moveController = _moveController;
        inputManager = _inputManager;
        cameraController = _cameraController;

        Observable.EveryUpdate().Subscribe(x => { commandInvoker.ExecuteCommands(); });
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
}
