using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    GameObject model;
    Rigidbody myRigidbody;
    MoveController moveController;

    public Rigidbody Rigidbody { get => myRigidbody; private set => myRigidbody = value; }
    public GameObject Model { get => model; private set => model = value; }
    PhysicsObject Physics { get; set; }

    public bool HitFloor
    {
        get
        {
            return Physics.HitFloor;
        }
    }

    [Inject]
    public void Construct(MoveController _moveController)
    {
        Model = this.gameObject;
        moveController = _moveController;
        Rigidbody = Model.GetComponent<Rigidbody>();
        Physics = this.GetComponent<PhysicsObject>();
    }

    public void MoveInDir(Vector3 dir)
    {
        Physics.AddDirToQueue(dir);
    }
}
