using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class RigidBodyNoiseStimulus : NoiseStimulus
{
    AudioManager audioManager;
    float volume = 0f;
    public enum NoiseImpulseLevel { none, low, middle, high }

    [Header("Ranges")]
    public int lowRange;
    public int middleRange;
    public int highRange;

    [Header("Impulse Clamps")]
    public int lowImpulse;
    public int middleImpulse;
    public int highImpulse;

    [Header("Game Audio Volume")]
    [Range(0,1)] public float lowVolume = 0f;
    [Range(0, 1)] public float middleVolume = 0.5f;
    [Range(0, 1)] public float highVolume = 1f;

    [Space]
    [Header("Breakable Object Values: OPTIONAL")]
    public bool breakable = false;
    public float breakImpulse = 0f;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    private NoiseImpulseLevel GetImpulse(float strength)
    {
        NoiseImpulseLevel noiseImpulseLevel = NoiseImpulseLevel.none;

        if (strength < lowImpulse) noiseImpulseLevel = NoiseImpulseLevel.none;
        else if (strength < middleImpulse) noiseImpulseLevel = NoiseImpulseLevel.low;
        else if (strength < highImpulse) noiseImpulseLevel = NoiseImpulseLevel.middle;
        else if (strength >= highImpulse) noiseImpulseLevel = NoiseImpulseLevel.high;

        return noiseImpulseLevel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Location = gameObject.transform;
        float strength = collision.impulse.y;
        
        NoiseImpulseLevel noiseImpulseLevel = GetImpulse(strength);

        switch (noiseImpulseLevel)
        {
            case NoiseImpulseLevel.none:
                intensity = 0f;
                volume = 0f;
                break;
            case NoiseImpulseLevel.low:
                intensity = lowImpulse;
                volume = lowVolume;
                break;
            case NoiseImpulseLevel.middle:
                volume = middleVolume;
                intensity = middleImpulse;
                break;
            case NoiseImpulseLevel.high: 
                volume = highVolume;
                intensity = highImpulse;
                break;
        }

        if (audioManager != null)
        {
            audioManager.SetVolume("Collision", volume);
            audioManager.PlaySound("Collision");
        }
        base.Emit();
    }

    public void Break()
    {
        if (!breakable) return;

        Location = gameObject.transform;
        intensity = breakImpulse;
        base.Emit();
    }
}
