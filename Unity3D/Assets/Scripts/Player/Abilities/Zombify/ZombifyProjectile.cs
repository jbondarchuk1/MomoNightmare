using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class ZombifyProjectile : Projectile
{
    public EnemyStateManager connectedESM;
    public bool isSecond = false;
    private Vector3 secondLocation = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSecond)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                connectedESM = collision.collider.gameObject.GetComponentInParent<EnemyManager>().esm;
                if (!connectedESM.canZombify)
                {
                    connectedESM = null;
                    Dissipate();
                }
                // stick projectile to enemy
                else if (!attached)
                {
                    StickToObject(collision);
                }
            }
            else
            {
                Dissipate();
            }
        }
        else
        {
            secondLocation = transform.position;
            StickToObject(collision);
            Debug.Log(secondLocation);
        }
    }

    public EnemyStateManager Zombify()
    {
        gameObject.SetActive(false);
        return connectedESM;
    }
    public Vector3 GetZombieDest()
    {
        return secondLocation;
    }
}
