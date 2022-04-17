using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : Patrol
{
    // parent of patrol points is separate to allow separate static transform.
    private float originalInnerRadius = 0f;
    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        originalInnerRadius = fov.currFOVValues.innerRadius;
        fov.currFOVValues.innerRadius = fov.currFOVValues.radius; // make the enemy very aware
        return base.RunCurrentState(enm,fov);
    }

    public void DeescalateAlert()
    {
        fov.currFOVValues.innerRadius = originalInnerRadius;
    }

}
