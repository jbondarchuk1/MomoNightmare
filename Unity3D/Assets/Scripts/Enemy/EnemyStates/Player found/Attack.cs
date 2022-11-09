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
        private bool Attacked = false;
        [SerializeField] private float coolDown = 3f;
        private float endTime = 0f;
    #endregion Private

    #region Start and Update


    private void Start()
    {
        PlayerRef = GameObject.Find("Player");
    }


    #endregion Start and Update

    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        if (!Attacked)
        {
            enm.Stop();
            AttackObject();
        }
        else if (TimeMethods.GetWaitComplete(endTime))
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

    // TODO add animation to this

    /// <summary>
    /// Handles attacking the current attacked game object.
    /// Returns true when the attack is finished
    /// </summary>
    /// <returns></returns>
    private void AttackObject()
    {
        endTime = TimeMethods.GetWaitEndTime(coolDown);
        Attacked = true;
        if (PlayerRef == AttackedObject) DamagePlayer();
    }
    private void DamagePlayer(int damage = 10)
    {
        PlayerManager pm = PlayerManager.Instance;
        PlayerStats stats = pm.statManager;
        stats.health -= damage;
    }
    public override void ExitState()
    {
        Attacked = false;
        AttackedObject = null;
    }


}
