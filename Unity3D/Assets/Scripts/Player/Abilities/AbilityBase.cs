using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

/// <summary>
/// </summary>
public abstract class AbilityBase : MonoBehaviour
{
    public Transform Cam { protected get; set; }
    public abstract Abilities Ability { get; }
    public abstract IEnumerator HandleAbility();
    public abstract void EnterAbility();
    public abstract void ExitAbility();
    public StarterAssetsInputs _inputs { get; set; }


}
