using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : Attack
{
    public new void InitializeState(StateInitializationData data)
    {
        base.InitializeState(data);
        enemyAnimationEventHandler.OnGrunt += Grunt;
    }

    public new void ExitState()
    {
        base.ExitState();
        enemyAnimationEventHandler.OnGrunt -= Grunt;
    }

    private void Grunt()
    {
        audioManager.PlaySound("Grunt");
    }
}
