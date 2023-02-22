using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the select pool object, usually a particle system
/// that toggles on and off. This is NOT the actual selected object.
/// </summary>
public class Select : MonoBehaviour, IPooledObject
{
    ParticleSystem particle;
    public bool selected = false;

    private void Awake()
    {
        if (particle == null) particle = GetComponentInChildren<ParticleSystem>();
    }

    public void OnObjectSpawn()
    {
        selected = true;
        gameObject.SetActive(true);
        particle.Play();
    }
    public void OnObjectDie()
    {
        selected = false;
        gameObject.SetActive(false);
        particle.Stop();
    }
    public void Follow(Transform follow)
    {
        if (follow != null)
        {
            this.transform.parent = follow;
            this.transform.position = follow.position;
        }
    }
    public void Follow(Transform follow, Vector3 worldPos)
    {
        if (follow != null)
        {
            this.transform.parent = follow;
            this.transform.position = worldPos;
        }
    }

}
