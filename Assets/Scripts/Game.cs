using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System.Threading.Tasks;

public class Game : MonoBehaviour
{

    private bool isSystemOnPause = false;

    public bool IsSystemOnPause
    {
        get
        {
            return isSystemOnPause;
        }
    }

    static Game instance;
    private ICommandInvoker commandInvoker;
    public LogsManager LogsManager
    {
        get;
        private set;
    }
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
    public void Constructor(ICommandInvoker invoker, MoveController _moveController, InputManager _inputManager, CameraController _cameraController, ParticlesManager _particlesManager, PlayerController playerController, LogsManager logsManager)
    {
        instance = this;
        commandInvoker = invoker;
        MoveControl = _moveController;
        Inputs = _inputManager;
        Camera = _cameraController;
        Particles = _particlesManager;
        Player = playerController;
        LogsManager = logsManager;

#if !DEBUGMODE
        Observable.EveryFixedUpdate().Where(x => !IsSystemOnPause).Subscribe(x => { commandInvoker.ExecuteCommands(); }).AddTo(this);
#endif
    }

    private void Start()
    {
        EventBus.StartListening(EventMessage.Pause, PauseGame);
        EventBus.StartListening(EventMessage.Unpause, UnPauseGame);
    }

    public void AddCommand(ICommand command)
    {
        if(!commandInvoker.IsUndoing() && !IsSystemOnPause)
        commandInvoker.Add(command);
    }

    public void UndoAll()
    {
        commandInvoker.UndoUntilTimestamp(0, Time.time);
    }

    public void OnDestroy()
    {
        commandInvoker.Dispose();
        EventBus.StopListening(EventMessage.Pause, PauseGame);
        EventBus.StopListening(EventMessage.Unpause, UnPauseGame);
    }

    public void SaveLogs()
    {
        LogsManager.SaveLogs(commandInvoker.ExportLogReportCommands());
    }

    private void PauseGame(object obj)
    {
        isSystemOnPause = true;
    }

    private void UnPauseGame(object obj)
    {
        isSystemOnPause = false;
    }
}
