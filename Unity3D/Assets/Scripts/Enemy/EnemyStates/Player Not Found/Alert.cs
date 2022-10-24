using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

public class Alert : Patrol
{
    public override StateEnum StateEnum { get; } = StateEnum.Alert;

    #region Exposed In Editor

        public float duration = 100f;

    #endregion Exposed In Editor

    #region Private

        private bool startingAlert = false;
        private float originalInnerRadius = 0f;

    #endregion Private

    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (startingAlert)
        {
            startingAlert = false;
            originalInnerRadius = fov.currFOVValues.innerRadius;
            fov.currFOVValues.innerRadius = fov.currFOVValues.radius; // make the enemy very aware
        }
        if (TimeMethods.GetWaitComplete(endTime))
            return ExitState(fov);

        return new StateInitializationData(StateEnum);
    }
    public override void InitializeState(StateInitializationData data)
    {
        endTime = TimeMethods.GetWaitEndTime(duration);
        startingAlert = true;
        base.InitializeState(data);
    }
    public StateInitializationData ExitState(FOV fov)
    {
        endTime = 0f;
        fov.currFOVValues.innerRadius = originalInnerRadius;
        return new StateInitializationData(StateEnum.Patrol);
    }


}
