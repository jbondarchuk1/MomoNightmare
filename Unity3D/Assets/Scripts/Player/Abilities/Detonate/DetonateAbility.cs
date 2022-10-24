using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static TimeMethods;

public class DetonateAbility : ProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Detonate;

    #region Private
    private DetonatorProjectile shotProjectile0;
    #endregion Private

    #region Start and Update
    private void Start()
    {
        ammo = 3f;
    }
    
    #endregion Start and Update

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        HandleWait();

        // projectile has been detonated but we still have a non null reference to it
        if (shotProjectile0 != null)
            if (shotProjectile0.gameObject.activeInHierarchy == false) shotProjectile0 = null;

        if (_inputs.isActionAndAiming())
        {
            _inputs.ResetActionInput();

            if (GetWaitComplete(endTime))
            {
                // shoot
                if (GetWaitComplete(endTime) && shotProjectile0 == null)
                    shotProjectile0 = (DetonatorProjectile)ShootObject(projectilePrefab);
                // detonate
                else DetonateProjectile();
            }
        }
        yield return wait;
    }
    private float DetonateProjectile()
    {
        shotProjectile0.GetComponent<DetonatorProjectile>().ActivateProjectile();
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
        this.shotProjectile0 = null;
    }

    public override void ExitAbility()
    {
        this.shotProjectile0 = null;
    }
}
