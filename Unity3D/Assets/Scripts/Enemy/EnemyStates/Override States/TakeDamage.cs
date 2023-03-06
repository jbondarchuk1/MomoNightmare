using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;
using static LayerManager;

public class TakeDamage : State
{
    public bool exit = false;
    EnemyManager em;
    public StateEnum previousState;

    private void Start()
    {
        em = GetComponentInParent<EnemyManager>();
    }

    public override StateEnum StateEnum { get; } = StateEnum.TakeDamage; 

    public override void ExitState() 
    {
        Debug.Log("Exiting state");
        em.esm.Overrides.canAggro = true;
        em.enemyAnimationEventHandler.OnEndDamage -= OnStandUp;
        exit = false;
    }

    public override void InitializeState(StateInitializationData data) 
    {
        List<StateEnum> toAlert = new List<StateEnum> { StateEnum.Patrol, StateEnum.SearchPatrol };
        em.esm.Overrides.canAggro = false;
        previousState = data.State != this.StateEnum ? data.State: StateEnum.Alert;
        if (toAlert.Contains(previousState)) previousState = StateEnum.Alert;
        em.enemyAnimationEventHandler.OnEndDamage += OnStandUp;
        em.animator.SetBool("isFallOver", true);
        em.animator.SetBool("isDamaged", true);
    }

    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity) { return new StateInitializationData(StateEnum); }

    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(NavMeshSpeed);
        Debug.Log("running damage state");
        enm.Stop();
        if (exit) return new StateInitializationData(previousState);
        return new StateInitializationData(StateEnum);
    }

    private void OnStandUp()
    {
        exit = true;
    }
}