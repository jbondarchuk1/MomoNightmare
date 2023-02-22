using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectManager : MonoBehaviour, IPooledObject
{
    public ParticleSystem explosionSystem;
    public void OnObjectSpawn()
    {
        explosionSystem.Play();
    }
}
