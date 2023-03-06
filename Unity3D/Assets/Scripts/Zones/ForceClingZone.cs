using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceClingZone : Zone
{
    ClingController playerCling;
    private new void Start()
    {
        playerCling = PlayerManager.Instance.playerMovementManager._groundedMovementController.clingController;
    }
    protected new void OnTriggerEnter(Collider other)
    {
        // base.OnTriggerEnter(other);
        playerCling.ForceCling = true;
    }
    protected new void OnTriggerExit(Collider other)
    {
        // base.OnTriggerExit(other);
        playerCling.ForceCling = false;
    }
}
