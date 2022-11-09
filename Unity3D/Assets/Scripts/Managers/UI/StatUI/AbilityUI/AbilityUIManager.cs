using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static AbilityUI;

public class AbilityUIManager : MonoBehaviour
{
    public Abilities ActiveAbility { get; set; }

    private Dictionary<Abilities, AbilityUI> abilities = new Dictionary<Abilities, AbilityUI>();
    private void Start()
    {
        foreach (AbilityUI ui in transform.GetComponentsInChildren<AbilityUI>())
            abilities.Add(ui.Ability, ui);

    }
    public void SetActiveAbility(Abilities ability)
    {
        SetAbilityState(abilities[ActiveAbility], AbilityUIState.Off);
        ActiveAbility = ability;
        SetAbilityState(abilities[ActiveAbility], AbilityUIState.Active);
    }
    private void SetAbilityState(AbilityUI abilityUI, AbilityUIState state)
    {
        abilityUI.SetUIState(state);
    }
}
