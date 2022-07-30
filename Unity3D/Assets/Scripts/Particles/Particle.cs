using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float lifespan = 5f;
    private float deathTime = Mathf.Infinity;
    public bool stopEmitting = true;
    public Vector3 spawnLocation;
    public Transform followTarget;

    private void Start()
    {
        if (spawnLocation == null)
        {
            spawnLocation = Vector3.zero;
        }
        else
        {
            transform.position = spawnLocation;
        }
    }
    // Update is called once per frame
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
            GameObject.Destroy(gameObject);
        }
    }

    public void TriggerDeath()
    {
        deathTime = Time.time + lifespan;
    }
}
