using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : Particle
{
    // Start is called before the first frame update
    void Start()
    {
        TriggerDeath();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnLocation != null)
        {
            transform.position = spawnLocation;
        }
        base.HandleParticle();
    }


}
