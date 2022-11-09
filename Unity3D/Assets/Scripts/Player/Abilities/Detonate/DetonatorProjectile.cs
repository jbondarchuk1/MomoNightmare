using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class DetonatorProjectile : Projectile
{
    private void Update()
    {
        if (GetWaitComplete(endTime) && attached == false)
            DeleteProjectile();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyManager em))
        {
            if (!em.canDetonate)
                DeleteProjectile();
            else if (!attached)
                StickToObject(collision);
        }
        else DeleteProjectile();
    }

    public override void ActivateProjectile()
    {
        gameObject.SetActive(false);
        ExplodeEnemy explode = attachedObject.GetComponentInParent<ExplodeEnemy>();
        explode.Explode();
    }
}
