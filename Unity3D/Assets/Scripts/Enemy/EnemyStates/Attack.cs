using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        return this;
    }
}
