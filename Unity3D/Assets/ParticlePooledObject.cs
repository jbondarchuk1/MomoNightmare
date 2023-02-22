using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePooledObject : MonoBehaviour, IPooledObject
{
    ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void OnObjectSpawn()
    {
        particle.Play();
    }
}
