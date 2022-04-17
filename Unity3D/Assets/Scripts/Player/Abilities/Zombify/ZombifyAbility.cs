using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;
public class ZombifyAbility : ProjectileAbility
{
    private bool attached1;
    public GameObject projectile1;

    // Update is called once per frame
    void Update()
    {
        HandleTime();
        if (projectile != null)
        {
            // check if projectile still exists
            if (projectile.activeInHierarchy == false)
                DissipateAll();
            // grab the attached bool if the projectile is still around
            else if (!attached)
                attached = projectile.GetComponent<ZombifyProjectile>().attached;
            // if we've fired the second projectile we need to see if this was also attached
            else if (attached && projectile1 != null)
                attached1 = projectile1.GetComponent<ZombifyProjectile>().attached;

        }
        StartCoroutine(HandleZombify());
    }

    private IEnumerator HandleZombify()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (attached1)
        {
            ZombifyObject();
        }
        else if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;
            if (GetWaitComplete(endTime) && projectile1 == null)
                ShootObject(projectilePrefab);
        }

        yield return wait;
    }

    private void ZombifyObject()
    {
        EnemyStateManager esm = projectile.GetComponent<ZombifyProjectile>().Zombify();
        Vector3 zombieDest = projectile1.GetComponent<ZombifyProjectile>().GetZombieDest();

        esm.overrides.Zombify(zombieDest);
        
        DissipateAll();

        endTime = 0f;
        projectile = null;
        attached = false;
        attached1 = false;
        Debug.Log("Zombified Enemy");
    }

    protected new void ShootObject(GameObject obj)
    {
        GameObject shot = base.ShootObject(obj);
        if (projectile == null)
            projectile = shot;
        else
        {
            projectile1 = shot;
            projectile1.GetComponent<ZombifyProjectile>().isSecond = true;
        }
    }

    private void HandleTime()
    {
        if (projectile != null)
        {
            ZombifyProjectile first = projectile.GetComponent<ZombifyProjectile>();
            if (projectile1 != null)
            {
                ZombifyProjectile second = projectile1.GetComponent<ZombifyProjectile>();
                first.endTime = second.endTime;
            }
            if (!attached && GetWaitComplete(first.endTime))
            {
                DissipateAll();
            }
        }
    }

    private void DissipateAll()
    {
        if (projectile != null)
        {
            GameObject.Destroy(projectile);
            projectile = null;
        }
        if (projectile1 != null)
        {
            projectile1.GetComponent<ZombifyProjectile>().Dissipate();
            GameObject.Destroy(projectile1);
            projectile1 = null;
        }
        attached = false;
    }
}
