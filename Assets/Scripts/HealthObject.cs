using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class HealthObject : MonoBehaviour
{
    [SerializeField]
    public int health = 10;

    [SerializeField]
    List<GameObject> receiversOfDestruction;

    private void Start()
    {
        Observable.EveryUpdate().Where(x => health <= 0).Subscribe(x => { SendSignalDestruction();});
    }


    private void SendSignalDestruction()
    {
        foreach (GameObject gameObject in receiversOfDestruction)
        {
            EventBus.TriggerEvent(EventMessage.Death, gameObject);
        }
    }

    private void Damage(int amount)
    {
        health -= amount;
    }
}
