using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

public class SpiderAttack : Attack
{
    public override StateEnum StateEnum { get; } = StateEnum.Attack;

    [SerializeField] private Animator proceduralAnimator;

    public override void ExitState()
    {
        base.ExitState();
        enemyAnimationEventHandler.OnAttack -= EnableProceduralAnimation;
        proceduralAnimator.enabled = true;
    }

    public override void InitializeState(StateInitializationData data)
    {
        base.InitializeState(data);
        proceduralAnimator.enabled = false;
        enemyAnimationEventHandler.OnAttack += EnableProceduralAnimation;
    }

    public void EnableProceduralAnimation()
    {
        Debug.Log("Enabling animator");
        proceduralAnimator.enabled = true;
    }
}
