using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoiseStimulus : NoiseStimulus
{
    public PlayerStats stats;

    public override void Emit()
    {
        intensity = stats.sound;
        base.Emit();
    }
}
