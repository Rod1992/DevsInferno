using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleInstance : MonoBehaviour
{
    [SerializeField]
    ParticleType type;

    public static ParticleInstance CreateInstance(ParticleType type, Quaternion rotation, Vector3 pos, Transform parent = null)
    {
        return GameObject.Instantiate<ParticleInstance>(Resources.Load<ParticleInstance>("Prefabs/Particles/" + type.ToString()), pos, rotation ,parent);
    }
}
