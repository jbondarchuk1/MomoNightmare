using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Follow()
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
            particle.Stop();
            gameObject.SetActive(false);
            follow = null;
        }
        else if (selected && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            particle.Play();
        }
        Follow();
        
    }
}
