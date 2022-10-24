using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoConsumable : Consumable
{
    [SerializeField] private int value = 1;
    [SerializeField] private AbilitiesManager.Abilities Ability;
    public override GameObject ContactObject { get; set; }
    
    public override void Consume(GameObject contactObject)
    {
        AbilitiesManager am = contactObject.GetComponent<PlayerManager>().abilitiesManager;
        am.PickUpAmmo(Ability, value);
        GameObject.Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            Consume(collider.gameObject);
        }
    }
}
