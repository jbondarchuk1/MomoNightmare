using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilitiesManager : MonoBehaviour
{
    public enum Abilities { Pickup, Scan, Zombify, Detonate, Pop, Teleport, Light }

    #region Private
        private StarterAssetsInputs _inputs;
        private List<AbilityBase> abilities;
        private int abilityIdx = 0;
        private StatUIManager statUIManager;
        private Dictionary<Abilities, AbilityBase> abilityMap = new Dictionary<Abilities, AbilityBase>();
    #endregion Private

    #region Start and Update
    void Start()
    {
        _inputs = StarterAssetsInputs.Instance;
        statUIManager = PlayerManager.Instance.statUIManager;

        abilities = new List<AbilityBase>();

        // TODO: Place all Abilities onto the parent object with the manager
        // use transform.GetComponentsInChildren<AbilityBase>();
        for (int i = 0; i < transform.childCount; i++)
        {
            AbilityBase ability;
            transform.GetChild(i).TryGetComponent(out ability);
            if (ability != null)
            {
                abilities.Add(ability);
                abilityMap.Add(ability.Ability, ability);
            }
        }
        abilities[abilityIdx].gameObject.SetActive(true);

    }

    void Update()
    {
        if      (_inputs.menuFState)
        {
            GetActiveAbility().ExitAbility();
            IncrementAbility();
            GetActiveAbility().EnterAbility();

            _inputs.menuFState = false;
            statUIManager.SetActiveAbility(abilityIdx);
        } // increment
        else if (_inputs.menuBState)
        {
            GetActiveAbility().ExitAbility();
            DecrementAbility();
            GetActiveAbility().EnterAbility();

            _inputs.menuBState = false;
            statUIManager.SetActiveAbility(abilityIdx);
        } // decrement

        StartCoroutine(GetActiveAbility().HandleAbility());
    }
    #endregion Start and Update

    public void PickUpAmmo(Abilities ability, int value)
    {
        AbilityBase projectileAbility = abilityMap[ability];

        if (typeof(ProjectileAbility) == projectileAbility.GetType())
            ((ProjectileAbility)projectileAbility).PickUpAmmo(value);
    }
    public AbilityBase GetActiveAbility()
    {
        AbilityBase activeAbility = abilities[abilityIdx];
            return activeAbility;
    }
    private void DecrementAbility()
    {
        abilities[abilityIdx].gameObject.SetActive(false);
        DecrementAbilityIdx();
        abilities[abilityIdx].gameObject.SetActive(true);
    }
    private void DecrementAbilityIdx()
    {
        if (abilityIdx <= 0)
        {
            abilityIdx = abilities.Count - 1;
            abilityIdx = abilityIdx < 0 ? 0 : abilityIdx;
        }
        else
        {
            abilityIdx -= 1;
        }
    }
    private void IncrementAbility()
    {
        abilities[abilityIdx].gameObject.SetActive(false);
        IncrementAbilityIdx();
        abilities[abilityIdx].gameObject.SetActive(true);
    }
    private void IncrementAbilityIdx()
    {
        if (abilityIdx >= abilities.Count - 1) abilityIdx = 0;
        else abilityIdx += 1;
    }

}
