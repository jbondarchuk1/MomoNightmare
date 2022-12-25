using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class ZombifyProjectile : Projectile, IPooledObject
{
    public bool isSecond = false;
    private EnemyStateManager attachedEnemyStateManager;
    public Vector3 SecondProjectileLocation { get; set; } = Vector3.zero;

    public void OnObjectSpawn()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isSecond)
        {
            if (collision.gameObject.TryGetComponent(out EnemyManager em))
            {
                if (!em.canZombify) DeleteProjectile();
                else if (!attached)
                {
                    attachedEnemyStateManager = em.esm;
                    StickToObject(collision);
                }
            }
            else DeleteProjectile();
        }
        else
        {
            SecondProjectileLocation = transform.position;
            StickToObject(collision);
        }
    }

    public override void ActivateProjectile()
    {
        try
        {
            EnemyStateManager enemy = attachedEnemyStateManager;
            Vector3 zombieDest = GetZombieDest();
            enemy.Overrides.Zombify(zombieDest);
        }
        catch(Exception ex)
        {
            Debug.LogError("enemy state manager inaccessable");
            Debug.LogException(ex);
        }
    }
    public Vector3 GetZombieDest()
    {
        return SecondProjectileLocation;
    }
}
