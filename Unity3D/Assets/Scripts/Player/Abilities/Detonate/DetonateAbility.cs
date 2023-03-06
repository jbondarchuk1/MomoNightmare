using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;
using static TimeMethods;
using EZCameraShake;

public class DetonateAbility : PhysicalProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Detonate;
    [SerializeField] private string explosionTag = "Explosion";

    #region Private
    private DetonatorProjectile shotProjectile0;
    #endregion Private

    #region Start and Update
    private new void Start()
    {
        base.Start();
    }
    
    #endregion Start and Update

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);

        bool isShooting = _inputs.actionPressed && GetWaitComplete(endTime);
        SetShootAnimation(isShooting && shotProjectile0 == null);

        // projectile has been detonated but we still have a non null reference to it
        if (shotProjectile0 != null)
        {
            if (shotProjectile0.gameObject.activeInHierarchy == false) shotProjectile0 = null;

            if (isShooting && shotProjectile0.attachedObject != null)
                DetonateProjectile();
        }


        yield return wait;
    }
    private float DetonateProjectile()
    {
        MakeBoomBoomEffect();
        shotProjectile0.ActivateProjectile();
        shotProjectile0.DeleteProjectile();
        shotProjectile0 = null;
        return GetWaitEndTime(hitTimer);
    }

    public override void EnterAbility()
    {
        PlayerAnimationEventHandler.OnShoot += Shoot;
    }
    public override void ExitAbility()
    {
        PlayerAnimationEventHandler.OnShoot -= Shoot;
    }

    public override void Shoot()
    {
        if (shotProjectile0 == null)
        {
            PlayerManager.Instance.audioManager.PlaySound("ProjectileSpawn", "Detonate");
            shotProjectile0 = (DetonatorProjectile)ShootObject();
        }
    }
    private void MakeBoomBoomEffect() => ObjectPooler.SpawnFromPool(explosionTag, shotProjectile0.transform.position, shotProjectile0.transform.rotation);
}
