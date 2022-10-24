using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;

/// <summary>
/// 
/// </summary>
public class Chase : State
{
    public override StateEnum StateEnum { get; } = StateEnum.Chase;

    #region Exposed In Editor

        public float navMeshSpeed = 2f;

    #endregion Exposed In Editor

    #region Private

        private GameObject PlayerRef;
        private GameObject AttackedObject;

    #endregion Private

    #region Start and Update
    private void Start()
    {
        PlayerRef = GameObject.Find("Player");
    }
    #endregion Start and Update

    /// <summary>
    /// Chase state returns either chase or search patrol of last seen position.
    /// </summary>
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);
        if (AttackedObject.activeInHierarchy) ChaseAfter(enm);
        return new StateInitializationData(StateEnum, AttackedObject);
    }
    public void InitializeState()
    {
        this.AttackedObject = PlayerRef;
    }
    public override void InitializeState(StateInitializationData data)
    {
        this.AttackedObject = data.Object;
    }
    public override void ExitState()
    {
        AttackedObject = null;
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    private void ChaseAfter(EnemyNavMesh enm)
    {
        enm.Chase(AttackedObject);
    }

}
