using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class Teleport : PhysicalProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Teleport;
    private PlayerManager playerManager;

    #region Private
    private TeleportProjectile shotProjectile0;
    #endregion Private

    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void TeleportTo(Transform target)
    {
        playerManager.TeleportTo(target);
    }

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (shotProjectile0 != null)
            if (!shotProjectile0.gameObject.activeInHierarchy) DissapateProjectile();

        if (_inputs.actionPressed && TimeMethods.GetWaitComplete(endTime))
        {
            endTime = TimeMethods.GetWaitEndTime(coolDownTimer); // timer after all shots is coolDownTimer
            // shoot
            if (shotProjectile0 == null)
                shotProjectile0 = (TeleportProjectile)ShootObject(projectilePrefab);
            
            // activate projectile
            else if (shotProjectile0.TeleportLocation != null)
            {
                TeleportTo(shotProjectile0.TeleportLocation);
                shotProjectile0.ActivateProjectile();
                DissapateProjectile();
                endTime = TimeMethods.GetWaitEndTime(hitTimer); // teleport timer is hitTimer
            }
            else DissapateProjectile();
        }

        yield return wait;
    }
    public override void EnterAbility()
    {
        this.shotProjectile0 = null;
    }
    public override void ExitAbility()
    {
        this.shotProjectile0 = null;
    }
    private void HandleWait()
    {
        if (shotProjectile0 != null)
            endTime = shotProjectile0.endTime;
    }

    private void DissapateProjectile()
    {
        this.shotProjectile0.DestroyProjectile();
        shotProjectile0 = null;
    }
}
