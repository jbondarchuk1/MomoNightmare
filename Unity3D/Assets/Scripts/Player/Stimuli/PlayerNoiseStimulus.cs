using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoiseStimulus : NoiseStimulus
{
    public PlayerStats stats;
    private void Update()
    {
        Location = PlayerManager.Instance.transform;
        Emit();
    }
    public override void Emit()
    {
        intensity = stats.sound;
        base.Emit();
    }
}
