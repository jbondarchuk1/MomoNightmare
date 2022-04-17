using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public bool setToIdle = false;
    public Patrol patrol;

    private void Start()
    {
        patrol = GameObject.Find("Patrol").GetComponent<Patrol>();
    }

    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (!setToIdle)
        {
            return patrol;
        }
        enm.StopNav();
        return this;
    }
}
