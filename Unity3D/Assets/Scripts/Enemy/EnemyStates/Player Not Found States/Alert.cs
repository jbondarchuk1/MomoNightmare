using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

public class Alert : Patrol
{
    public override StateEnum StateEnum { get; } = StateEnum.Alert;

    #region Exposed In Editor


    #endregion Exposed In Editor

    #region Private
        private int alertIdx = -1; 
    #endregion Private
    private new void Start()
    {
        base.Start();
    }
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (alertIdx == -1) foreach (FOV.FOVValues val in fov.fovValues) if (val.name == "Alert") alertIdx = fov.fovValues.IndexOf(val);
        base.RunCurrentState(enm, fov);
        enm.SetSpeed(NavMeshSpeed);
        fov.currFOVIdx = alertIdx;
        return new StateInitializationData(StateEnum);
    }
    public override void InitializeState(StateInitializationData data)
    {
        base.InitializeState(data);
    }
    public StateInitializationData ExitState(FOV fov)
    {
        fov.currFOVIdx = 0;
        return new StateInitializationData(StateEnum.Patrol);
    }
}
