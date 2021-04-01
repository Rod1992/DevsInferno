using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleInstance : MonoBehaviour
{
    [SerializeField]
    ParticleType type;

    public static void CreateInstance(ParticleType type, Transform parent)
    {
        GameObject.Instantiate<ParticleInstance>(Resources.Load<ParticleInstance>("Prefabs/Particles/" + type.ToString()), parent);
    }
}
