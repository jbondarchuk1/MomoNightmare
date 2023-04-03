using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Chase : Node
    {
        [SerializeField] float chaseDistance = 20f;
        public override NodeState Evaluate()
        {
            Debug.Log("Chasing");
            enemyNavMesh.SetSpeed(1);

            if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) < chaseDistance)
                state = NodeState.RUNNING;
            else state = NodeState.FAILURE;

            if (state == NodeState.RUNNING) enemyNavMesh.Chase(PlayerManager.Instance.gameObject);
            else enemyNavMesh.Chase(null);

            return state;
        }
    }
}

