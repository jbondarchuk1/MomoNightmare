using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static TimeMethods;

public class DetonateAbility : PhysicalProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Detonate;

    #region Private
    private DetonatorProjectile shotProjectile0;
    #endregion Private

    #region Start and Update
    private new void Start()
    {
        base.Start();
        ammo = 3f;
    }
    
    #endregion Start and Update

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        HandleWait();

        bool isShooting = _inputs.actionPressed && GetWaitComplete(endTime);
        SetShootAnimation(isShooting && shotProjectile0 == null);

        // projectile has been detonated but we still have a non null reference to it
        if (shotProjectile0 != null)
            if (shotProjectile0.gameObject.activeInHierarchy == false) shotProjectile0 = null;

        if (shotProjectile0 == null) yield return wait;

        if (isShooting && shotProjectile0.attachedObject != null)
        {
            endTime = GetWaitEndTime(hitTimer);
            DetonateProjectile();
        }
        yield return wait;
    }
    private float DetonateProjectile()
    {
        shotProjectile0.ActivateProjectile();
        shotProjectile0.DeleteProjectile();
        shotProjectile0 = null;
        return GetWaitEndTime(hitTimer);
    }
    private void HandleWait()
    {
        if (shotProjectile0 != null)
            endTime = shotProjectile0.endTime;
    }
    public override void EnterAbility()
    {
        PlayerAnimationEventHandler.OnShoot += Shoot;
        this.shotProjectile0 = null;
    }
    public override void ExitAbility()
    {
        PlayerAnimationEventHandler.OnShoot -= Shoot;
        this.shotProjectile0 = null;
    }

    public override void Shoot()
    {
        if (shotProjectile0 == null)
            shotProjectile0 = (DetonatorProjectile)ShootObject();
    }
}
