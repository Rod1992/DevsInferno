using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    
    MovingObject movingObject;

    public MovingObject MovingObject { get => movingObject;}

    [Inject]
    public void Construct()
    {

    }

    
}
