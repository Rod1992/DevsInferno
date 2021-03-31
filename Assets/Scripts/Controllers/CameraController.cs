using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class CameraController
{
    //from 100 to 0
    public const int SENSIBILITY = 20; 

    

    [Inject]
    public void Constructor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Observable.EveryFixedUpdate().Where(x => MouseGoingLeft()).Subscribe(x => new RotateCommand(true));
        Observable.EveryFixedUpdate().Where(x => MouseGoingRight()).Subscribe(x => new RotateCommand(false));
    }

    public bool MouseGoingLeft()
    {
        return Screen.width * SENSIBILITY / 100  > Input.mousePosition.x;
    }

    public bool MouseGoingRight()
    {
        return Screen.width * (100 - SENSIBILITY) / 100 < Input.mousePosition.x;
    }
}


