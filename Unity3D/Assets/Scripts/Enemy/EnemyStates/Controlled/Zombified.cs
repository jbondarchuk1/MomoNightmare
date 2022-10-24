using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;
using static EnemySoundListener;

public class Zombified : State
{
    #region Exposed In Editor
    
        [SerializeField] private float Duration = 100f;

    #endregion Exposed In Editor

    #region Private

        private float endTime = 0f;
        private Vector3 Destination = Vector3.zero;
        private GameObject AttackedObject;

    #endregion Private
    public override StateEnum StateEnum { get; } = StateEnum.Zombify;
    public override void InitializeState(StateInitializationData data)
    {
        endTime = TimeMethods.GetWaitEndTime(Duration);
        if (data.Object != null) AttackedObject = data.Object;
        else if (data.Location != null) Destination = data.Location;
    }
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (TimeMethods.GetWaitComplete(endTime))
        {

        }
        return new StateInitializationData(this.StateEnum);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    public override void ExitState()
    {
        AttackedObject = null;
        Destination = Vector3.zero;
        endTime = 0f;
    }

    
}
