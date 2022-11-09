using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// Intermediary class between thirdpersoncharacter and navmesh
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField] private ThirdPersonCharacter character;
    [SerializeField] private NavMeshAgent agent;
    private EnemyNavMesh enm;

    void Start()
    {
        agent.updateRotation = false;
        agent.updatePosition = true;

        agent = GetComponent<NavMeshAgent>();
        enm = GetComponent<EnemyNavMesh>();
        character.maxSpeed = GetMaxSpeed();
    }
    private float GetMaxSpeed()
    {
        float max = 0f;
        foreach (State state in GetComponentsInChildren<State>())
            if (state.NavMeshSpeed > max) max = state.NavMeshSpeed;
        return max;
    }
    void Update()
    {

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);

        else character.Move(Vector3.zero, false, false);


    }
}
