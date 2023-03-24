using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

/// <summary>
/// Due to the architecture surrounding states, I want the EnemyStateManager to have
/// easy access initializing any given state. This means that StateOverrides needs to have
/// a general data container for any relevant information that a state needs to start.
/// 
/// ex: SearchPatrol needs a suspicious location to search
/// ex: Attack/Chase need a gameObject to chase
/// </summary>
public class StateInitializationData
{
    public StateEnum State { get; set; } = StateEnum.None;
    public StateEnum State1 { get; set; } = StateEnum.None;
    public Vector3 Location { get; set; } = Vector3.zero;
    public GameObject Object { get; set; } = null;
    public StateInitializationData()
    {
        State = StateEnum.None;
    }
    public StateInitializationData(StateEnum state)
    {
        State = state;
    }
    public StateInitializationData(StateEnum state, StateEnum state1)
    {
        State = state;
        State1 = state1;
    }
    public StateInitializationData(StateEnum state, Vector3 location)
    {
        State = state;
        Location = location;
    }
    public StateInitializationData(StateEnum state, GameObject obj)
    {
        State = state;
        Object = obj;
    }
}
