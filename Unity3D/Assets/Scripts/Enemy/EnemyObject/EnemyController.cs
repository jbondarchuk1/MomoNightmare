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
    public ICharacter character;
    [SerializeField] protected NavMeshAgent agent;

    protected void Start()
    {
        character = GetComponentInChildren<ICharacter>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    protected void Update()
    {
        if (character == null) return;
        agent.enabled = character.IsGrounded;
        if (!agent.enabled)
        {
            character.Move(Vector3.zero, false, false);
            return;
        }


        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);

        else character.Move(Vector3.zero, false, false);
    }
}
