using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseStimulus : Stimulus
{
    public Transform location;
    public LayerMask soundMask;
    [HideInInspector] public float endTime = Mathf.Infinity;

    public override void Emit()
    {
        if (Time.time < endTime)
        {
            if (location == null) location = transform;
            if (intensity == 0f || location == null) return;

            Collider[] objects = Physics.OverlapSphere(location.position, intensity, soundMask);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].name.Contains("Enemy"))
                {
                    EnemySoundListener listener = objects[i].GetComponentInChildren<EnemyManager>().SoundListener;
                    if (listener != null) listener.Listen(location.position, (int)intensity);
                }
            }
        }
    }
}
