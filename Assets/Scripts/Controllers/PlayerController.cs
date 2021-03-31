using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    GameObject model;
    MoveController moveController;

    [Inject]
    public void Construct()
    {
        model = GameObject.Instantiate(Resources.Load<GameObject>("Male_01_V01"), Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    public GameObject Model()
    {
        return model;
    }
}
