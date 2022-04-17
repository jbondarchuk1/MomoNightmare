using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class DetonatorProjectile : Projectile
{
    public EnemyStateManager connectedESM;
    private void Update()
    {
        if (GetWaitComplete(endTime) && attached == false)
        {
            Dissipate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            connectedESM = collision.collider.gameObject.GetComponentInParent<EnemyManager>().esm;
            if (!connectedESM.canDetonate)
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
            Dissipate();
    }

    public void Detonate()
    {
        gameObject.SetActive(false);
        ExplodeEnemy explode = attachedObject.GetComponentInParent<ExplodeEnemy>();
        explode.Explode();
    }
}
