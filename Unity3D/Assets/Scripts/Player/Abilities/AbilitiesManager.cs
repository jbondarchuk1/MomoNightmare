using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilitiesManager : MonoBehaviour
{
    [HideInInspector]public bool canUseAbility = true;
    public enum Abilities { Pickup, Scan, Zombify, Detonate, Pop, Teleport, Light }
    [SerializeField] private Abilities ActiveAbility;

    [Header("Projectile Ability Settings")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform activationOrigin;
    [SerializeField] private Transform shootOrigin;

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

        foreach (AbilityBase ability in transform.GetComponentsInChildren<AbilityBase>())
        {
            if (ability is ProjectileAbility)
                ((ProjectileAbility)ability).ShootOrigin = shootOrigin;
            ability._inputs = _inputs;
            ability.Cam = cam;
            abilities.Add(ability);
            abilityMap.Add(ability.Ability, ability);
        }

        ActiveAbility = abilities[abilityIdx].Ability;
    }

    void Update()
    {
        if      (_inputs.menuFState)
        {
            GetActiveAbility().ExitAbility();
            IncrementAbilityIdx();
            GetActiveAbility().EnterAbility();
            _inputs.menuFState = false;
            ActiveAbility = GetActiveAbility().Ability;
            statUIManager.SetActiveAbility(ActiveAbility);
        } // increment

        else if (_inputs.menuBState)
        {
            GetActiveAbility().ExitAbility();
            DecrementAbilityIdx();
            GetActiveAbility().EnterAbility();

            _inputs.menuBState = false;
            statUIManager.SetActiveAbility(GetActiveAbility().Ability);
            ActiveAbility = abilities[abilityIdx].Ability;

        } // decrement

        if (canUseAbility)
            StartCoroutine(GetActiveAbility().HandleAbility());
        statUIManager.SetActiveAbility(GetActiveAbility().Ability);
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

    private void DecrementAbilityIdx()
    {
        if (abilityIdx <= 0)
        {
            abilityIdx = abilities.Count - 1;
            abilityIdx = abilityIdx < 0 ? 0 : abilityIdx;
        }
        else abilityIdx -= 1;
    }

    private void IncrementAbilityIdx()
    {
        if (abilityIdx >= abilities.Count - 1) abilityIdx = 0;
        else abilityIdx += 1;
    }

    public AbilityBase GetAbility(Abilities ability)
    {
        AbilityBase a;
        abilityMap.TryGetValue(ability, out a);
        return a;
    }
}
