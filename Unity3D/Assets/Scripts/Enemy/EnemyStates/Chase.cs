using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * CURRENTLY CHASE IS EQUIVALENT TO AN ATTACK STATE
 * ALL ATTACK STATE REFERENCES HAVE BEEN COMMENTED OUT
 */
public class Chase : State
{
    private EnemyStateManager esm;
    // aggro means keep chasing, else searchpatrol
    public bool aggro = true;
    public float navMeshSpeed = 2f;

    private State nextState;
    
    [Header("Available Next States")]
    private Idle idle;
    private SearchPatrol searchPatrol;
    private Attack attack;

    private void Start()
    {
        esm = GetComponentInParent<EnemyStateManager>();
        idle = esm.idle;
        searchPatrol = esm.searchPatrol;
        attack = esm.attack;
    }

    /// <summary>
    /// Chase state returns either chase or search patrol of last seen position.
    /// </summary>
    /// <returns></returns>
    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);

        chase(enm, fov);
        if (aggro)
        {
            nextState = this;
        }
        else
        {
            searchPatrol.Reset();
            searchPatrol.checkLocation = GameObject.Find("Player").transform.position;
            nextState = searchPatrol;
        }
        return nextState;
    }

    // Handle Hitboxes and Aggro for the chase state
    void chase(EnemyNavMesh enm, FOV fov)
    {
        if (fov.playerInRange && aggro)
        {
            enm.hitboxActive = true;
            enm.AttackNav();
            aggro = true;
        }
        else
        {
            enm.hitboxActive = false;
            aggro = false;
        }
    }
}
