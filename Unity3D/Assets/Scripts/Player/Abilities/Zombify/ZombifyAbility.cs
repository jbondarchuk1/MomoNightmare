using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static TimeMethods;


public class ZombifyAbility : ProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Zombify;


    #region Private
        private ZombifyProjectile shotProjectile0;
        private ZombifyProjectile shotProjectile1;
    #endregion Private

    #region Start and Update
    private new void Start()
    {
        ammo = 4f;
        base.Start();
    }
    #endregion Start and Update

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);


        HandleTime();
        if (shotProjectile0 != null)
        {
            // check if projectile still exists
            if (shotProjectile0.gameObject.activeInHierarchy == false)
            {
                if (ammo % 2 != 0) ammo--;
                DissipateAllProjectiles();
            }
        }

        if (shotProjectile1 != null)
            if (shotProjectile1.attached)
                ZombifyStuckObject();

        if (_inputs.isActionAndAiming())
        {
            _inputs.ResetActionInput();

            if (GetWaitComplete(endTime) && shotProjectile0 == null)
                shotProjectile0 = ShootZombifyProjectile();
        }


        yield return wait;
    }
    public override void EnterAbility()
    {
        DissipateAllProjectiles();
    }
    public override void ExitAbility()
    {
        DissipateAllProjectiles();
    }

    private void ZombifyStuckObject()
    {
        ZombifyProjectile projectileOnEnemy = ((ZombifyProjectile)shotProjectile0);
        projectileOnEnemy.ActivateProjectile();
        DissipateAllProjectiles();

        endTime = GetWaitEndTime(hitTimer);
        shotProjectile0 = null;
        shotProjectile1 = null;
    }
    private ZombifyProjectile ShootZombifyProjectile()
    {
        try
        {
            ZombifyProjectile shot = (ZombifyProjectile)base.ShootObject(projectilePrefab);
            if (shotProjectile0 == null) return shot;

            shot.isSecond = true;
            return shot;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
    }
    private void HandleTime()
    {
        float dissolveTime = 0f;

        if      (shotProjectile1 != null) dissolveTime = shotProjectile1.endTime;
        else if (shotProjectile0 != null) dissolveTime = shotProjectile0.endTime;
        
        if (TimeMethods.GetWaitComplete(dissolveTime)) DissipateAllProjectiles();
    }
    private void DissipateAllProjectiles()
    {
        if (shotProjectile0 != null)
        {
            GameObject.Destroy(shotProjectile0.gameObject);
            shotProjectile0 = null;
        }
        if (shotProjectile1 != null)
        {
            GameObject.Destroy(shotProjectile1.gameObject);
            shotProjectile1 = null;
        }
    }

}
