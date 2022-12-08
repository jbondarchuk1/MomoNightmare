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
    public Transform follow = null;

    public void OnObjectSpawn()
    {
        selected = true;
    }
    public void OnObjectDie()
    {
        selected = false;
    }
    private void Follow()
    {
        if (follow != null)
            this.transform.position = follow.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!selected && gameObject.activeInHierarchy)
        {
            if (particle != null) particle.Stop();
            gameObject.SetActive(false);
            follow = null;
        }
        else if (selected && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            if (particle != null) particle.Play();
        }
        Follow();
        
    }
}
