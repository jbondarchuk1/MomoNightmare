using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitiesManager;

public class Teleport : ProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Teleport;

    #region Private
    private TeleportProjectile shotProjectile0;
    #endregion Private

    // TODO: Implement this method
    // I don't know yet what things need to happen to move the player to a teleport object
    private void TeleportTo(Vector3 location)
    {

    }

    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (_inputs.isActionAndAiming())
        {
            // shoot
            _inputs.ResetActionInput();
            if (shotProjectile0 == null)
                shotProjectile0 = (TeleportProjectile)ShootObject(projectilePrefab);
            
            // activate projectile
            else
            {
                TeleportTo(shotProjectile0.TeleportLocation);
                shotProjectile0.ActivateProjectile();
                shotProjectile0 = null;
            }
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
}
