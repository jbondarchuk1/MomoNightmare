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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);

        else character.Move(Vector3.zero, false, false);
    }
}
