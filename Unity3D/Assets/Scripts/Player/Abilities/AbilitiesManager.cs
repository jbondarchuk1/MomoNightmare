using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilitiesManager : MonoBehaviour
{
    public enum Abilities { Pickup, Scan, Zombify, Detonate, Pop };

    public StarterAssetsInputs _inputs;
    private List<Transform> abilities;
    private int abilityIdx = 0;
    [SerializeField] StatUIManager statUIManager;
    // Start is called before the first frame update
    void Start()
    {
        abilities = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            abilities.Add(transform.GetChild(i));
        }
        abilities[abilityIdx].gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.menuFState)
        {
            IncrementAbility();
            _inputs.menuFState = false;
            statUIManager.SetActiveAbility(abilityIdx);
            
        }
        else if (_inputs.menuBState)
        {
            DecrementAbility();
            _inputs.menuBState = false;
            statUIManager.SetActiveAbility(abilityIdx);
        }
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
        if (abilityIdx >= abilities.Count - 1)
        {
            abilityIdx = 0;
        }
        else
        {
            abilityIdx += 1;
        }
    }

    public void FoundAmmo(Abilities ability, int value)
    {
        abilities[((int)ability)].gameObject.GetComponent<ProjectileAbility>().FoundAmmo(value);
    }

    public ProjectileAbility GetActiveProjectileAbility()
    {
        if (abilities[abilityIdx].TryGetComponent<ProjectileAbility>(out ProjectileAbility activeAbility))
        {
            return activeAbility;
        }

        return null;
    }
}
