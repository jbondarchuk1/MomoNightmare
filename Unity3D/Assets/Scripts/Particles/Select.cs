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
        if (!selected && gameObject.active)
        {
            particle.Stop();
            gameObject.SetActive(false);
            Debug.Log("Deselected");
            follow = null;
        }
        else if (selected && !gameObject.active)
        {
            gameObject.SetActive(true);
            particle.Play();
            Debug.Log("Selected");
        }
        Follow();
        
    }
}
