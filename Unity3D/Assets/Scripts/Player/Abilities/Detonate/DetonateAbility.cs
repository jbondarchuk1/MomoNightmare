using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class DetonateAbility : ProjectileAbility
{
    private void Update()
    {
        StartCoroutine(HandleDetonate());
    }

    private IEnumerator HandleDetonate()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);

        if (projectile != null)
        {
            // check if projectile still exists
            if (projectile.activeInHierarchy == false)
            {
                attached = false;
                GameObject.Destroy(projectile);
                projectile = null;
            }
            // grab the attached bool if the projectile is still around
            else if (!attached)
                attached = projectile.GetComponent<DetonatorProjectile>().attached;
        }

        if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;
            if (GetWaitComplete(endTime) && projectile == null)
                projectile = ShootObject(projectilePrefab);
            else if (GetWaitComplete(endTime) && attached && projectile != null)
                DetonateObject(projectile);
        }
        yield return wait;
    }

    private void DetonateObject(GameObject instance)
    {
        endTime = 0f;
        projectile.GetComponent<DetonatorProjectile>().Detonate();
        GameObject.Destroy(projectile);
        projectile = null;
        attached = false;
    }

}
