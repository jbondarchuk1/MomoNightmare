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
    private EnemyStateManager esm;

    public EnemyStats()
    {
        health = 100;
        stamina = 100;
        sound = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        esm = GetComponentInChildren<EnemyStateManager>();
        fov = GetComponent<FOV>();
    }

    void Update()
    {
        if (fov.sus && esm.currState == esm.patrol)
        {
            if (BuildAwareness())
            {
                esm.searchPatrol.checkLocation = fov.susLocation;
                esm.overrides.OverrideState(new SearchPatrol(), false);
                awareness = 0;
            }
        }
        else if (esm.currState == esm.patrol)
        {
            CoolAwareness();
        }
        else
        {
            awareness = 0;
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
