using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using static LayerManager;

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
        Vector3 pos = transform.position;
        pos.y += 2;
        agent.enabled = Physics.Raycast(pos, Vector3.down, 5, GetMask(Layers.Ground,Layers.Obstruction));
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
