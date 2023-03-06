using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonZone : Zone
{
    AimController playerAim;
    private new void Start()
    {
        playerAim = PlayerManager.Instance.playerMovementManager._aimController;
    }
    protected new void OnTriggerEnter(Collider other)
    {
        // base.OnTriggerEnter(other);
        playerAim.ForceFirstPerson = true;
    }
    protected new void OnTriggerExit(Collider other)
    {
        // base.OnTriggerExit(other);
        playerAim.ForceFirstPerson = false;
    }
}
