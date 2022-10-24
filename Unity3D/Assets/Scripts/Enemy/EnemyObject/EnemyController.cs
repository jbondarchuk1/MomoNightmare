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
    public ThirdPersonCharacter character;
    public NavMeshAgent navMesh;
    // Start is called before the first frame update
    void Start()
    {
        navMesh.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMesh.remainingDistance > navMesh.stoppingDistance)
        {
            character.Move(navMesh.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
        
    }
}
