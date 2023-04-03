using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Falling : Node
    {
        public override NodeState Evaluate()
        {
            if (!animator.GetBool("OnGround"))
                state = NodeState.RUNNING;
            else state = NodeState.FAILURE;

            if (state == NodeState.RUNNING)
                enemyNavMesh.Stop();


            return state;
        }
    }
}

