using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

[RequireComponent(typeof(ExplosionStimulus))]
[RequireComponent(typeof(SoundNoiseStimulus))]
public class ExplosionEffectManager : MonoBehaviour, IPooledObject
{
    public ParticleSystem explosionSystem;
    public float explosionRadius = 5f;
    public int force = 5;
    public int damage = 5;
    private ExplosionStimulus explosionStimulus;
    private SoundNoiseStimulus soundNoiseStimulus;
    private void Awake()
    {
        explosionStimulus = GetComponent<ExplosionStimulus>();  
        soundNoiseStimulus = GetComponent<SoundNoiseStimulus>();
    }
    public void OnObjectSpawn()
    {
        this.gameObject.SetActive(true);

        explosionSystem.Play();
        CameraShaker.Instance.Shake(CameraShakePresets.Explosion);
        explosionStimulus.Emit();
        soundNoiseStimulus.Emit();
    }
}
