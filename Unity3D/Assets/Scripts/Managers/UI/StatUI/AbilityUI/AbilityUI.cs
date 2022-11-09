using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class AbilityUI : MonoBehaviour
{
    public enum AbilityUIState { Off, Active, Inactive }
    public AbilityUIState state = AbilityUIState.Off;
    public Abilities Ability;
    
    
    [SerializeField] private GameObject ActiveUI;
    [SerializeField] private GameObject InactiveUI;

    public void SetUIState(AbilityUIState state)
    {
        this.state = state;
        switch (this.state)
        {
            case AbilityUIState.Active:
                ActiveUI.SetActive(true);
                InactiveUI.SetActive(false);
                break;
            case AbilityUIState.Inactive:
                ActiveUI.SetActive(false);
                InactiveUI.SetActive(true);
                break;
            case AbilityUIState.Off:
                ActiveUI.SetActive(false);
                InactiveUI.SetActive(false);
                break;
        }
    }
}
