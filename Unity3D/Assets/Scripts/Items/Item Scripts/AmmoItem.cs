using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

[RequireComponent(typeof(Projectile))]
public class AmmoItem : Item
{
    [Space]
    [Space]
    [Header("Ammo Vals")]
    [SerializeField] private Abilities ability;
    [SerializeField] private int ammoCount = 1;
    Projectile projectile;
    [HideInInspector] public bool canObtain = false;
    private void Start()
    {
        TryGetComponent(out projectile);
    }
    public override void Obtain()
    {
        if (!canObtain) return;
        Debug.Log("Obtaining");
        AbilityBase ammoTo = PlayerManager.Instance.abilitiesManager.GetAbility(ability);
        if (ammoTo is ProjectileAbility) ((ProjectileAbility)ammoTo).PickUpAmmo(1);
        projectile.DeleteProjectile();
        canObtain = false;
    }
}
