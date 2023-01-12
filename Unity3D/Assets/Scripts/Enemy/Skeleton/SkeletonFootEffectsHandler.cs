using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFootEffectsHandler : FootEffectsHandler
{
    [SerializeField] private EnemyAnimationEventHandler _enemyAnimationEventHandler;
    private new void Start()
    {
        base.Start();
        _enemyAnimationEventHandler.OnStepL += StepL;
        _enemyAnimationEventHandler.OnStepR += StepR;
    }
    protected override void Step(bool isL = true)
    {
        audioManager.SetVolume("Footstep", 1f);
        if (isL) audioManager.PlaySound("Footstep", "StepL");
        else audioManager.PlaySound("Footstep", "StepR");
    }
    protected override void OnDisable()
    {
        _enemyAnimationEventHandler.OnStepL -= StepL;
        _enemyAnimationEventHandler.OnStepR -= StepR;
    }
}
