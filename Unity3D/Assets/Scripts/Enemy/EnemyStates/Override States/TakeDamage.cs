using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;
using static LayerManager;

public class TakeDamage : State
{
    public bool exit = false;
    EnemyManager em;
    public StateEnum previousState = StateEnum.Patrol;

    private void Start()
    {
        em = GetComponentInParent<EnemyManager>();
    }

    public override StateEnum StateEnum { get; } = StateEnum.TakeDamage; 

    public override void ExitState() 
    {
        em.esm.Overrides.canAggro = true;
        em.enemyAnimationEventHandler.OnEndDamage -= OnStandUp;
        exit = false;
        previousState = StateEnum.None;
    }

    public override void InitializeState(StateInitializationData data) 
    {
        List<StateEnum> toAlert = new List<StateEnum> { StateEnum.Patrol, StateEnum.SearchPatrol };
        em.esm.Overrides.canAggro = false;
        previousState = data.State1 != this.StateEnum ? data.State1: StateEnum.Alert;
        if (toAlert.Contains(previousState)) previousState = StateEnum.Alert;
        em.enemyAnimationEventHandler.OnEndDamage += OnStandUp;
        em.animator.SetBool("isFallOver", true);
        em.animator.SetBool("isDamaged", true);
        
    }

    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity) { return new StateInitializationData(StateEnum); }

    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(0);
        enm.Chase(null);
        
        enm.Stop();
        if (exit)
        {
            if (previousState == StateEnum.Chase || previousState == StateEnum.Attack) return new StateInitializationData(StateEnum.Chase, PlayerManager.Instance.gameObject);
            return new StateInitializationData(previousState);
        }
        return new StateInitializationData(StateEnum);
    }

    private void OnStandUp()
    {
        exit = true;
    }
}