using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public enum ParticleType
{
    None = 0,
    TimeTwist = 1
}

public class ParticlesManager : MonoBehaviour
{

    List<ParticleInstance> particles = new List<ParticleInstance>();
   
    [Inject]
    public void Constructor()
    {
        EventBus.StartListening(EventMessage.StartRollBackTime, CreateTimeTwist);
        EventBus.StartListening(EventMessage.EndRollBackTime, (param) => { CleanParticles(); });
    }

    public void Add(ParticleInstance particle)
    {
        particles.Add(particle);
    }


    public void CleanParticles()
    {
        foreach(ParticleInstance particle in particles)
        {
            Destroy(particle);
        }

        particles.Clear();
    }

    public void CreateTimeTwist(System.ParamArrayAttribute param)
    {
        ParticleInstance.CreateInstance(ParticleType.TimeTwist, this.transform);
    }

    public void OnDestroy()
    {
        EventBus.StopListening(EventMessage.StartRollBackTime, CreateTimeTwist);
        EventBus.StopListening(EventMessage.EndRollBackTime, (param) => { CleanParticles(); });
    }
}
