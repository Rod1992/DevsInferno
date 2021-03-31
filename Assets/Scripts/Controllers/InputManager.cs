using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class InputManager
{
    [Inject]
    public void Constructor()
    {
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.UpArrow)).Subscribe(x => new MoveCommand(MoveType.Forward));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.DownArrow)).Subscribe(x => new MoveCommand(MoveType.Backward));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.LeftArrow)).Subscribe(x => new MoveCommand(MoveType.Left));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.RightArrow)).Subscribe(x => new MoveCommand(MoveType.Right));

        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.R)).Subscribe(x => Game.Instance.UndoAll());
    }
}
