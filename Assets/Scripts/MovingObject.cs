using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    float speed = 0.01f;
    [SerializeField]
    public float speedRotation = 0.2f;

    GameObject model;
    Rigidbody myRigidbody;

    public Rigidbody Rigidbody { get => myRigidbody; private set => myRigidbody = value; }
    public GameObject Model { get => model; private set => model = value; }

    PhysicsObject PhysicsObject { get; set; }

    public bool HitFloor
    {
        get
        {
            return PhysicsObject.HitFloor;
        }
    }

    [Inject]
    public void Construct()
    {
        Model = this.gameObject;
        Rigidbody = Model.GetComponent<Rigidbody>();
        PhysicsObject = this.GetComponent<PhysicsObject>();
    }

    public void MoveInDir(Vector3 dir)
    {
        PhysicsObject.AddDirToQueue(dir);
    }

    public void MoveUpwardsOrDownWard(bool isDown)
    {
        this.MoveInDir(Model.transform.up * speed * 20 * (isDown ? -1 : 1));
    }

    public void MoveLeftOrRight(bool isLeft)
    {
        this.MoveInDir(Model.transform.right * speed * (isLeft ? -1 : 1));
    }

    public void MoveForwardOrBackwards(bool isBackWards)
    {
        this.MoveInDir((Model.transform.forward * speed * (isBackWards ? -1 : 1)));
    }

    public void Rotate(bool isLeft)
    {
        Quaternion currentRotation = Model.transform.rotation;
        Vector3 euler = currentRotation.eulerAngles + new Vector3(0, speedRotation * (isLeft ? -1 : 1), 0);
        Rigidbody.MoveRotation(Quaternion.Euler(euler));
    }

    public void ApplyGravity(bool reverse = false)
    {
        this.MoveInDir((Physics.gravity * speed * (reverse ? -1 : 1)));
    }
}
