using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    // public EnemyStateManager esm;
    public abstract State RunCurrentState(EnemyNavMesh enm, FOV fov);
}