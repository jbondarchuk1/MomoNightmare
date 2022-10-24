using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemySoundListener;
using static EnemyStateManager;

public abstract class State: MonoBehaviour
{
    public abstract StateEnum StateEnum { get; }
    public abstract void InitializeState(StateInitializationData data);
    public abstract StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov);
    public abstract StateInitializationData Listen(Vector3 soundOrigin, int intensity);
    public abstract void ExitState();
}