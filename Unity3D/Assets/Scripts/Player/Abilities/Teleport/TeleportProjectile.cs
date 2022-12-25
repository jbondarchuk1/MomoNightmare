using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportProjectile : Projectile
{
    public Transform TeleportLocation { get; set; } = null;
    public override void ActivateProjectile()
    {
        DeleteProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Teleport to: " + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out TeleportObject to))
        {
            if (!attached)
            {
                StickToObject(collision);
                TeleportLocation = to.TeleportationTarget;
            }
            else DeleteProjectile();
        }
    }

}
