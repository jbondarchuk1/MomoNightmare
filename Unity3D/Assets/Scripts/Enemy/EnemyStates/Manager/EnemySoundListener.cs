using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

/// <summary>
/// Intermediary Class between State Manager and States that handle have methods to handle sounds they hear
/// </summary>
public class EnemySoundListener 
{
    private StateOverrides stateOverrides;
    private EnemyStateManager enemyStateManager; // keep track of current state

    public EnemySoundListener(StateOverrides overrides, EnemyStateManager enemyStateManager)
    {
        this.stateOverrides = overrides;
        this.enemyStateManager = enemyStateManager;
    }

    public void Listen(Vector3 soundOrigin, int intensity)
    {
        StateEnum stateListenResult = enemyStateManager.GetState().Listen(soundOrigin, intensity).State;
        switch (stateListenResult)
        {
            case StateEnum.SearchPatrol:
                stateOverrides.Search(soundOrigin);
                return;
        }

    }
}
