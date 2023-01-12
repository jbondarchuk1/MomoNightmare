using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNoiseStimulus : NoiseStimulus
{
    [field: SerializeField] public string SoundGroupName { get; set; } = "";
    [field: SerializeField] public string SoundName { get; set; } = "";
    [SerializeField] private AudioManager audioManager;


    public new void Emit()
    {
        base.Emit();
        if (SoundName != "")
            audioManager.PlaySound(SoundGroupName, SoundName);
        else audioManager.PlaySound(SoundGroupName);
    }
}
