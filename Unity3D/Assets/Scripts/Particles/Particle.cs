using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour, IPooledObject
{
    public float lifespan = 5f;
    private float deathTime = Mathf.Infinity;
    public bool stopEmitting = true;
    public Transform followTarget;

    public void OnObjectSpawn()
    {

    }
    protected void HandleParticle()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position;
        }
        HandleDeath();
    }

    protected void HandleDeath()
    {
        if (Time.time > deathTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void TriggerDeath()
    {
        deathTime = Time.time + lifespan;
    }
}
