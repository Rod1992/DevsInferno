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
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.UpArrow) || Input.GetKey(GetKeyUpForCulture())).Subscribe(x => new MoveCommand(MoveType.Forward));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.DownArrow) || Input.GetKey(GetKeyDownForCulture())).Subscribe(x => new MoveCommand(MoveType.Backward));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(GetKeyLeftForCulture())).Subscribe(x => new MoveCommand(MoveType.Left));
        Observable.EveryFixedUpdate().Where(x => Input.GetKey(KeyCode.RightArrow) || Input.GetKey(GetKeyRightForCulture())).Subscribe(x => new MoveCommand(MoveType.Right));
        Observable.EveryFixedUpdate().Where(x => Input.GetKeyDown(KeyCode.Space)).Subscribe(x => new JumpCommand(1));

        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.R)).Subscribe(x => Game.Instance.UndoAll());

        
    }

    //TODO : Get Real Keyboard Layout instead of language
    //Not Perfect as it's not they keyboard layout(native code) but not a bad patch
    public KeyCode GetKeyUpForCulture()
    {
        switch (Application.systemLanguage)
        {
            //azerty
            case SystemLanguage.French:
                return KeyCode.A;
            //qwerty
            default:
                return KeyCode.W;
        }
    }

    public KeyCode GetKeyLeftForCulture()
    {
        switch (Application.systemLanguage)
        {
            //azerty
            case SystemLanguage.French:
                return KeyCode.Q;
            //qwerty
            default:
                return KeyCode.A;
        }
    }

    public KeyCode GetKeyRightForCulture()
    {
        switch (Application.systemLanguage)
        {
            //maybe there is a culture with a different key
            default:
                return KeyCode.D;
        }
    }

    public KeyCode GetKeyDownForCulture()
    {
        switch (Application.systemLanguage)
        {
            default:
                return KeyCode.S;
        }
    }
}
