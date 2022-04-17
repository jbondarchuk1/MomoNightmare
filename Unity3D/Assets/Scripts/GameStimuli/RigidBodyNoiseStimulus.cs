using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyNoiseStimulus : NoiseStimulus
{
    [Header("Ranges")]
    public int shortRange;
    public int mediumRange;
    public int longRange;

    [Header("Impulse Clamps")]
    public int shortImpulse;
    public int mediumImpulse;
    public int longImpulse;

    private void OnCollisionEnter(Collision collision)
    {
        location = gameObject.transform;
        float strength = collision.impulse.y;

        if (strength < shortImpulse)
            intensity = 0f;
        else if (strength < mediumImpulse)
            intensity = shortRange;
        else if (strength < longImpulse)
            intensity = mediumRange;
        else if (strength >= longImpulse)
            intensity = longRange;

        base.Emit();
    }
}
