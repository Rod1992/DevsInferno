using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class PhysicsObject : MonoBehaviour
{
    List<ContactPoint> savedContacts;
    List<Vector3> dirsToTreat;
    new Rigidbody rigidbody;

    public bool HitFloor
    {
        get;
        set;
    }

    [Inject]
    public void Constructor()
    {
        savedContacts = new List<ContactPoint>();
        dirsToTreat = new List<Vector3>();
        rigidbody = this.GetComponent<Rigidbody>();
        Observable.EveryFixedUpdate().Where(x => dirsToTreat.Count > 0).Subscribe(x => TreatDirs());
    }


    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        collision.GetContacts(contacts);

        HitFloor = false;
        foreach (ContactPoint contact in contacts)
        {
            savedContacts.Add(contact);
            if ((this.transform.position - contact.point).y < 0)
                HitFloor = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        collision.GetContacts(contacts);

        foreach (ContactPoint contact in contacts)
        {
            savedContacts.Remove(contact);

            if ((this.transform.position - contact.point).y < 0)
                HitFloor = false;
        }
    }

    public void AddDirToQueue(Vector3 dir)
    {
        dirsToTreat.Add(dir);
    }

    public void TreatDirs()
    {
        Vector3 sumDir = Vector3.zero;
        foreach (Vector3 vector in dirsToTreat)
        {
            sumDir += vector;
        }

        rigidbody.MovePosition(this.transform.position + sumDir);
        dirsToTreat.Clear();
    }
}
