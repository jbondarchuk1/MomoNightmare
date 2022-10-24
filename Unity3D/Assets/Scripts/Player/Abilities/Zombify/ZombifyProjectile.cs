using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class ZombifyProjectile : Projectile
{
    public bool isSecond = false;

    private EnemyStateManager attachedEnemyStateManager;
    private Vector3 secondLocation = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSecond)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyManager em = collision.collider.gameObject.GetComponentInParent<EnemyManager>();
                if (!em.canZombify) DeleteProjectile();
                else if (!attached) StickToObject(collision);
            }
            else DeleteProjectile();
        }
        else
        {
            secondLocation = transform.position;
            StickToObject(collision);
            Debug.Log(secondLocation);
        }
    }

    public override void ActivateProjectile()
    {
        EnemyStateManager enemy = attachedEnemyStateManager;
        Vector3 zombieDest = GetZombieDest();
        enemy.Overrides.Zombify(zombieDest);
    }
    public Vector3 GetZombieDest()
    {
        return secondLocation;
    }
}
