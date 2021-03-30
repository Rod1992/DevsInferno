using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class CameraController
{

    [Inject]
    public void Constructor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Observable.EveryFixedUpdate().Where(x => MouseGoingLeft()).Subscribe(x => new RotateCommand(true));
        Observable.EveryFixedUpdate().Where(x => MouseGoingRight()).Subscribe(x => new RotateCommand(false));
    }

    public bool MouseGoingLeft()
    {
        return Input.GetAxis("Mouse X") > 0;
    }

    public bool MouseGoingRight()
    {
        return Input.GetAxis("Mouse X") < 0;
    }
}


