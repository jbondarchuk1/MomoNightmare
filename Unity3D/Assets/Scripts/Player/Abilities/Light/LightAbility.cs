using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class LightAbility : AbilityBase
{
    public override Abilities Ability { get; } = Abilities.Light;

    private float endTime = Mathf.Infinity;

    private readonly StarterAssetsInputs _inputs = StarterAssetsInputs.Instance;
    [SerializeField] private GameObject LightGameObject;

    public override void EnterAbility()
    {
        SetLight(false);
        endTime = 0f;
    }

    public override void ExitAbility()
    {
        SetLight(false);
        endTime = 0f;
    }

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (TimeMethods.GetWaitComplete(endTime))
        {
            if (_inputs.isActionAndAiming())
            {
                _inputs.ResetActionInput();
                ToggleLight();
            }
        }
        yield return wait;
    }

    private void ToggleLight()
    {
        LightGameObject.SetActive(!LightGameObject.activeInHierarchy);
    }
    private void SetLight(bool on = false)
    {
        LightGameObject.SetActive(on);
    }
}