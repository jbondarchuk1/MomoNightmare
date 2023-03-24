using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportProjectile : Projectile, IPooledObject
{
    public Transform TeleportLocation { get; set; } = null;
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }
    public override void ActivateProjectile()
    {
        TeleportLocation = null;
        DeleteProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Teleport to: " + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out TeleportObject to))
        {
            if (!attached)
            {
                trail.emitting = false;
                StickToObject(collision);
                TeleportLocation = to.TeleportationTarget;
            }
            else
            {
                TeleportLocation = null;
                DeleteProjectile();
            }
        }
        else
        {
            TeleportLocation = null;
            DeleteProjectile();
        }
    }

    public void OnObjectSpawn()
    {
        trail.emitting = true;
    }
}
