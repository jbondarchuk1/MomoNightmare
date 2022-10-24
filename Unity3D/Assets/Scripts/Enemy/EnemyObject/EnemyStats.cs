using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [Header("Non Base")]
    public float maxAwareness = 100;
    public float awareness = 0;
    [Range(0,1)]public float awarenessFactor = 0.5f;

    private FOV fov;
    private StateOverrides stateOverrides;

    public EnemyStats()
    {
        health = 100;
        stamina = 100;
        sound = 0;
    }
    void Start()
    {
        fov = GetComponent<FOV>();
        stateOverrides = GetComponentInChildren<EnemyStateManager>().Overrides;
    }

    void Update()
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
                if (BuildAwareness()) stateOverrides.Search();
                break;
        }
    }

    public bool BuildAwareness()
    {
        float awarenessIncrement = Time.deltaTime / awarenessFactor;
        
        if (awareness + awarenessIncrement >= maxAwareness) { awareness = 0; return true; }
        else awareness += awarenessIncrement;

        return false;
    }
    public void CoolAwareness()
    {
        if (awareness == 0) return;
        else if (awareness < 0) awareness = 0;
        else awareness--;
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
