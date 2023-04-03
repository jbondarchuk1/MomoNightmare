using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviourTree
{
    public class Search : Node
    {
        public Vector3 location = Vector3.zero;
        public override NodeState Evaluate()
        {
            if (state != NodeState.SUCCESS) location = (Vector3)GetData("SusLocation");

            state = NodeState.RUNNING;
            enemyNavMesh.SetSpeed(.5f);
            enemyNavMesh.Patrol(location);

            if (Vector3.Distance(transform.position, location) < 2)
                return Exit();
            
            return state;
        }

        public NodeState Exit()
        {
            location = Vector3.zero;
            audioManager.PlaySound("Grunt");

            state = NodeState.SUCCESS;
            return state;
        }

    }
}