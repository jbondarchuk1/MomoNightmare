using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseStimulus : Stimulus
{
    public Transform Location { get; set; }

    public LayerMask activeSoundMask;
    [HideInInspector] public float endTime = Mathf.Infinity;

    public override void Emit()
    {
        if (Time.time < endTime)
        {
            if (Location == null) Location = transform;
            if (intensity == 0f || Location == null) return;

            Collider[] objects = Physics.OverlapSphere(Location.position, intensity, activeSoundMask);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].name.Contains("Enemy"))
                {
                    EnemySoundListener listener = objects[i].GetComponentInChildren<EnemyManager>().SoundListener;
                    if (listener != null) listener.Listen(Location.position, (int)intensity);
                }
            }
        }
    }
}
