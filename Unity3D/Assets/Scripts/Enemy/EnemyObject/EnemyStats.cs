using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [Header("Non Base")]
    public float maxAwareness = 100;
    public float awareness = 0;
    public bool isKnockedOver = false;
    [Range(0,1)]public float awarenessFactor = 0.5f;

    private FOV fov;

    public EnemyStats()
    {
        health = 100;
        stamina = 100;
        sound = 0;
    }
    void Start()
    {
        fov = GetComponent<FOV>();
    }

    public void HandleAwareness()
    {
        switch (fov.FOVStatus)
        {
            case FOV.FOVResult.Seen:
                awareness = 0;
                break;
            case FOV.FOVResult.Unseen:
                if (awareness > 0) CoolAwareness();
                break;
            case FOV.FOVResult.SusObject:
                break;
            case FOV.FOVResult.SusPlayer:
                if (isAware()) awareness = maxAwareness;
                else BuildAwareness();
                break;
        }
    }
    private void BuildAwareness()
    {
        float awarenessIncrement = Time.deltaTime / awarenessFactor;
        awareness += awarenessIncrement;
    }
    private void CoolAwareness()
    {
        if (awareness == 0) return;
        else if (awareness < 0) awareness = 0;
        else awareness--;
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
    public bool isAware()
    {
        return awareness >= maxAwareness;
    }
}
