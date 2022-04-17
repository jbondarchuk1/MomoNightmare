using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseStimulus : Stimulus
{
    public Transform location;
    public LayerMask soundMask;

    public override void Emit()
    {
        if (location == null) location = transform;
        if (intensity == 0f || location == null) return;
        
        Collider[] objects = Physics.OverlapSphere(location.position, intensity, soundMask);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name.Contains("Enemy"))
            {
                EnemyStateManager esm = objects[i].GetComponentInChildren<EnemyStateManager>();
                if (esm != null)
                {
                    esm.overrides.HandleSound(location.position, intensity);
                }
            }
        }
    }
}
