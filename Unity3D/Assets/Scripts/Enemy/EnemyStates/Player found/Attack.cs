using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;
using static EnemySoundListener;

public class Attack : State
{
    public override StateEnum StateEnum { get; } = StateEnum.Attack;

    #region Exposed
        [SerializeField] protected List<HandAttackHandler> handAttackHandlers = new List<HandAttackHandler>();
        [SerializeField] protected float coolDown = 3f;
        [SerializeField] protected Animator animator;
        [SerializeField] protected EnemyAnimationEventHandler enemyAnimationEventHandler;
        [SerializeField] protected AudioManager audioManager;
    #endregion Exposed

    #region Private
    protected GameObject AttackedObject;
        protected bool beganAttack = false;
        protected bool endedAttack = false;
        protected float endTime = 0f;
        protected int isAttackingHash;
    #endregion Private

    #region Start and Update
    protected void Start()
    {
        isAttackingHash = Animator.StringToHash("isAttacking");
    }
    #endregion Start and Update

    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(this.NavMeshSpeed);
        if (!beganAttack) AttackObject();

        if (TimeMethods.GetWaitComplete(endTime) && endedAttack)
            return ChangeState();

        return new StateInitializationData(StateEnum, AttackedObject);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    public override void InitializeState(StateInitializationData data)
    {
        Debug.Log("Initializing attack state");
        enemyAnimationEventHandler.OnAttack += ExitAttackAnimation;
        this.AttackedObject = data.Object;
    }

    /// <summary>
    /// Handles attacking the current attacked game object.
    /// Returns true when the attack is finished
    /// </summary>
    protected void AttackObject()
    {
        audioManager.Play("Attack");
        endTime = TimeMethods.GetWaitEndTime(coolDown);
        beganAttack = true;
        endedAttack = false;
        animator.SetBool(isAttackingHash, beganAttack);

        if (handAttackHandlers.Count > 0)
            foreach (HandAttackHandler handAttackHandler in handAttackHandlers)
                handAttackHandler.EnableHitbox();
    }
    public override void ExitState()
    {
        AttackedObject = null;
    }
    protected StateInitializationData ChangeState()
    {
        beganAttack = false;
        endedAttack = false;
        enemyAnimationEventHandler.OnAttack -= ExitAttackAnimation;
        return new StateInitializationData(StateEnum.Chase, AttackedObject);
    }
    public void ExitAttackAnimation()
    {
        animator.SetBool(isAttackingHash, false);

        endedAttack = true;

        if (handAttackHandlers.Count > 0)
            foreach (HandAttackHandler handAttackHandler in handAttackHandlers)
                handAttackHandler.DisableHitbox();
    }
}
