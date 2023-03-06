using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

[RequireComponent(typeof(AmmoItem))]
public class DetonatorProjectile : Projectile
{
    AmmoItem item;
    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), PlayerManager.Instance.GetComponent<Collider>());
        item = GetComponent<AmmoItem>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!attached && collision.collider.gameObject.layer != LayerManager.GetLayer(LayerManager.Layers.Target))
        {
            StickToObject(collision);
            item.canObtain = true;
        }
    }

    public override void ActivateProjectile()
    {
        gameObject.SetActive(false);
    }
}
