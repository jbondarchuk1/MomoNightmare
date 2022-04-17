using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOverrides : MonoBehaviour
{
    public EnemyStateManager esm;
    public bool canAggro = true;
    public bool canHear = true;
    public bool canSee = true;

    public void HandleRegularOverrides()
    {
        // ANY STATE CAN AGGRO
        if (CheckAggro() && esm.currState != esm.chase)
            Aggro();
    }

    public void Zombify(Vector3 location)
    {
        esm.searchPatrol.Reset();
        esm.SetOverrideState(esm.searchPatrol);
        esm.searchPatrol.checkLocation = location;
        canAggro = false;
    }

    /// <summary>
    /// Any sound stimulus should pass through here. This is the state handler for sounds.
    /// Only Patrol and SearchPatrol states care about sounds. Don't distract after already aggro'd
    /// </summary>
    public void HandleSound(Vector3 location, float intensity)
    {
        if (canHear)
        {
            State nextState;
            if (esm.currState == esm.patrol)
                nextState = ((Patrol)esm.currState).Listen(location, intensity);
            else if (esm.currState == esm.searchPatrol)
                nextState = ((SearchPatrol)esm.currState).Listen(location, intensity);
            else return;


            if (nextState == esm.chase)
                Aggro();
            else if (nextState == esm.searchPatrol && esm.currState != esm.searchPatrol)
            {
                esm.searchPatrol.checkLocation = location;
                esm.stateOverride = esm.searchPatrol;
            }
        }
    }

    private bool CheckAggro()
    {
        if (esm.fov.canSeePlayer && canSee && !esm.fov.targetNotPlayer)
            return true;
        return false;
    }

    public void Aggro()
    {
        if (canAggro)
        {
            esm.fov.canSeePlayer = true;
            esm.fov.patrolling = false;
            esm.fov.patrolPointInRange = false;
            esm.chase.aggro = true;
            esm.stateOverride = esm.chase;
        }
    }
    public void OverrideState(State state)
    {
        esm.SetOverrideState(state);
    }
    public void OverrideState(State state, bool trueReference)
    {
        esm.SetOverrideState(state, trueReference);
    }
}
