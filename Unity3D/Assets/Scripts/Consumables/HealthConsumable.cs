using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthConsumable : Consumable
{
    [SerializeField] private int value = 10;
    public override GameObject ContactObject { get; set; }
    public override void Consume(GameObject contactObject)
    {
        PlayerManager.Instance.statManager.HealPlayer(value);
        GameObject.Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("Player"))
        {
            Consume(collider.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) { }
}
