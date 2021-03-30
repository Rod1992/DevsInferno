using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class Game : MonoBehaviour
{
    static Game instance;
    public ICommandInvoker commandInvoker;
    public MoveController moveController;
    public InputManager inputManager;

    public static Game Instance
    {
        get
        {
            return instance;
        }
    }

    [Inject]
    public void Constructor(ICommandInvoker invoker, MoveController _moveController, InputManager _inputManager)
    {
        instance = this;
        commandInvoker = invoker;
        moveController = _moveController;
        inputManager = _inputManager;

        Observable.EveryUpdate().Subscribe(x => { commandInvoker.ExecuteCommands(); });
    }

}
