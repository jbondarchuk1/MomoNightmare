using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportProjectile : Projectile
{
    public Vector3 TeleportLocation { get; set; } = Vector3.zero;
    public override void ActivateProjectile()
    {
        DeleteProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent(out TeleportObject _))
            if (!attached)
            {
                StickToObject(collision);
                TeleportLocation = collision.transform.position;
            }
            else DeleteProjectile();
    }
}
