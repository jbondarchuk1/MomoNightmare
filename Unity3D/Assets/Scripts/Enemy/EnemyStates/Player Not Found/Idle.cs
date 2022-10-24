using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;

public class Idle : State
{
    public override StateEnum StateEnum { get; } = StateEnum.None;
    public override void InitializeState(StateInitializationData data) { }
    public override void ExitState() { }


    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        return new StateInitializationData(StateEnum.Patrol);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
}
