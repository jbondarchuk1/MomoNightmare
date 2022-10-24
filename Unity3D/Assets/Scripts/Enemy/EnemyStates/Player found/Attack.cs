using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;
using static EnemySoundListener;

public class Attack : State
{
    public override StateEnum StateEnum { get; } = StateEnum.Attack;

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


    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (AttackObject())
            return new StateInitializationData(StateEnum.Chase, AttackedObject);
        return new StateInitializationData(StateEnum, AttackedObject);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    public override void InitializeState(StateInitializationData data)
    {
        this.AttackedObject = data.Object;
    }
    
    // TODO Fill out method
    /// <summary>
    /// Handles attacking the current attacked game object.
    /// Returns true when the attack is finished
    /// </summary>
    /// <returns></returns>
    private bool AttackObject()
    {
        return true;
    }


    public override void ExitState()
    {
        AttackedObject = null;
    }

}
