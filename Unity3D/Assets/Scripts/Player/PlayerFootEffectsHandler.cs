using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using static StatRangeLevel;
using UnityEngine;

public class PlayerFootEffectsHandler : FootEffectsHandler
{
    private PlayerMovement movement;
    protected StarterAssetsInputs _input;

    private new void Start()
    {
        _input = StarterAssetsInputs.Instance;
        base.Start(); 
        PlayerAnimationEventHandler.OnStepL += StepL;
        PlayerAnimationEventHandler.OnStepR += StepR;
        movement = PlayerManager.Instance.playerMovementManager;
        audioManager = PlayerManager.Instance.audioManager;
    }

    protected override void Step(bool isL = true)
    {
        string soundName = FindGroundType();
        soundName = isL ? soundName + "L" : soundName + "R";

        Vector3 position = bottom.position;

        if (_input.sprint)
        {
            ObjectPooler.SpawnFromPool(Tag, position, Quaternion.identity);
            StartCoroutine(TraumaInducer.Instance.InduceStress(45, 0.04f));
        }
        float vol = 1f;
        switch (PlayerManager.Instance.statManager.StealthRange)
        {
            case Range.Low:
                vol = low;
                break;
            case Range.Middle:
                vol = middle;
                break;
            case Range.High:
                vol = high;
                break;
        }
        audioManager.SetVolume("Footsteps", vol);
        audioManager.PlaySound("Footsteps", soundName, true);
    }
    protected override void OnDisable()
    {
        PlayerAnimationEventHandler.OnStepL -= StepL;
        PlayerAnimationEventHandler.OnStepR -= StepR;
    }
}
