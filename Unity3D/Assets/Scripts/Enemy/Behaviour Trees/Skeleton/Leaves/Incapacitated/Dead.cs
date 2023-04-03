using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// 
    /// </summary>
    public class Dead : Node
    {
        public override NodeState Evaluate()
        {
            if (animator.GetBool("isDead") || enemyStats.health == 0)
                state = NodeState.RUNNING;

            if (state == NodeState.RUNNING)
            {
                enemyNavMesh.Stop();
                enemyUIManager.gameObject.SetActive(false);
            }

            return state;

        }
    }
}
