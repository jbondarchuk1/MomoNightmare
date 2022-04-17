using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodHandler : Particle
{
    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position;
            transform.rotation = followTarget.rotation;
        }
        base.HandleParticle();
    }
}
