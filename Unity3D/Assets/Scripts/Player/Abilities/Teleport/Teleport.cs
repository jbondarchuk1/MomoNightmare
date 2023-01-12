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

    private new void Start()
    {
        base.Start();
        playerManager = PlayerManager.Instance;
    }

    private void TeleportTo(Transform target)
    {
        playerManager.TeleportTo(target);
    }

    public override void Shoot()
    {
        if (shotProjectile0 == null)
        {
            PlayerManager.Instance.audioManager.PlaySound("ProjectileSpawn", "Teleport");
            shotProjectile0 = (TeleportProjectile)ShootObject();
        }
    }

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (shotProjectile0 != null)
            if (!shotProjectile0.gameObject.activeInHierarchy) DissapateProjectile();


        bool isShooting = _inputs.actionPressed && TimeMethods.GetWaitComplete(endTime);
        SetShootAnimation(isShooting && shotProjectile0 == null);

        if (isShooting && shotProjectile0 != null)
        {
            endTime = TimeMethods.GetWaitEndTime(coolDownTimer); // timer after all shots is coolDownTimer

            // activate projectile
            if (shotProjectile0.TeleportLocation != null)
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
        PlayerAnimationEventHandler.OnShoot += Shoot;
        this.shotProjectile0 = null;
    }
    public override void ExitAbility()
    {
        PlayerAnimationEventHandler.OnShoot -= Shoot;
        this.shotProjectile0 = null;
    }
    private void HandleWait()
    {
        if (shotProjectile0 != null)
            endTime = shotProjectile0.endTime;
    }

    private void DissapateProjectile()
    {
        this.shotProjectile0.DeleteProjectile();
        shotProjectile0 = null;
    }
}
