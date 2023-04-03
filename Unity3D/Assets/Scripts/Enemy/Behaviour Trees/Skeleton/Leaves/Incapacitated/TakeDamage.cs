using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class TakeDamage : Node
    {
        public override NodeState Evaluate()
        {
            if (enemyStats.isKnockedOver)
            {
                enemyGroupManager.Alert();
                state = NodeState.RUNNING;
                enemyStats.isKnockedOver = false;
                enemyAnimationEventHandler.OnEndDamage += OnStandUp;
                animator.SetBool("isFallOver", true);
                animator.SetBool("isDamaged", true);
            }

            if (state == NodeState.SUCCESS)
            {
                state = NodeState.FAILURE;
                return NodeState.SUCCESS;
            }
            if (state == NodeState.RUNNING)
            {
                enemyNavMesh.Stop();
            }

            return state;
            
        }
        private void OnStandUp()
        {
            state = NodeState.SUCCESS;
            enemyAnimationEventHandler.OnEndDamage -= OnStandUp;
            animator.SetBool("isFallOver", false);
            animator.SetBool("isDamaged", false);
        }
    }
}

