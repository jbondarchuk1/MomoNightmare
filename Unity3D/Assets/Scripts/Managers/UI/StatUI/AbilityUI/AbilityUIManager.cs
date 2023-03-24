using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static AbilityUI;

public class AbilityUIManager : MonoBehaviour
{
    public Abilities ActiveAbility { get; set; }

    private Dictionary<Abilities, AbilityUI> abilities = new Dictionary<Abilities, AbilityUI>();
    private void Awake()
    {
        AbilityUI[] abilityUIs = GetComponentsInChildren<AbilityUI>();
        foreach (AbilityUI ability in abilityUIs) abilities.Add(ability.Ability, ability);
    }
    public void SetActiveAbility(Abilities ability)
    {
        try
        {
            SetAbilityState(abilities[ActiveAbility], AbilityUIState.Off);
            ActiveAbility = ability;
            SetAbilityState(abilities[ActiveAbility], AbilityUIState.Active);
        }
        catch (Exception ex)
        {
            Debug.Log("Cannot get " + ActiveAbility.ToString() + " from dictionary");
        }
    }
    private void SetAbilityState(AbilityUI abilityUI, AbilityUIState state)
    {
        abilityUI.SetUIState(state);
    }
}
