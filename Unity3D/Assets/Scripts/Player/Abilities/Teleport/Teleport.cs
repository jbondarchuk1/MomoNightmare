using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class Teleport : PhysicalProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Teleport;
    [SerializeField] private string AuraTag = "TeleportAura";
    [SerializeField] private Transform footPosition;
    private PlayerManager playerManager;

    #region Private
    private TeleportProjectile shotProjectile0;
    #endregion Private

    private new void Start()
    {
        base.Start();
        playerManager = PlayerManager.Instance;
    }

    private IEnumerator TeleportTo(Transform target)
    {
        PlayerManager.Instance.playerMovementManager._groundedMovementController.crouchController.Crouch();
        PlayerManager.Instance.playerMovementManager.canMove = false;
        GameObject aura = ObjectPooler.SpawnFromPool(AuraTag, footPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        playerManager.uiManager.TransitionUIManager.Transition(true);
        yield return new WaitForSeconds(1);
        playerManager.TeleportTo(target);
        yield return new WaitForSeconds(.2f);
        playerManager.uiManager.TransitionUIManager.Transition(false);
        PlayerManager.Instance.playerMovementManager._groundedMovementController.standingController.Stand();
        PlayerManager.Instance.playerMovementManager.canMove = true;
        aura.SetActive(false);
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
                StartCoroutine(TeleportTo(shotProjectile0.TeleportLocation));
                shotProjectile0.ActivateProjectile();
            }
            DissapateProjectile();
        }

        yield return wait;
    }
    public override void EnterAbility()
    {
        PlayerAnimationEventHandler.OnShoot += Shoot;
    }
    public override void ExitAbility()
    {
        PlayerAnimationEventHandler.OnShoot -= Shoot;
    }

    private void DissapateProjectile()
    {
        this.shotProjectile0.DeleteProjectile();
        shotProjectile0 = null;
    }
}
