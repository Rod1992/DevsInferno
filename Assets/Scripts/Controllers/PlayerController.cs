using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController
{
    GameObject model;
    Rigidbody rigidbody;
    MoveController moveController;

    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public GameObject Model { get => model; set => model = value; }

    [Inject]
    public void Construct(MoveController _moveController)
    {
        Model = GameObject.Instantiate(Resources.Load<GameObject>("Male_01_V01"), Vector3.zero, Quaternion.Euler(0, 0, 0));
        moveController = _moveController;
        Rigidbody = Model.GetComponent<Rigidbody>();
    }

}
