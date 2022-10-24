using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchesConsumable : Consumable
{
    [SerializeField] private int value = 1;
    public override GameObject ContactObject { get; set; }
    public override void Consume(GameObject contactObject)
    {
        GameObject.Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Consume(collision.gameObject);
        }
    }
}
