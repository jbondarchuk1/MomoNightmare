using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Attack : Node
    {
        public override void Initialize()
        {
            base.Initialize();
            enemyAnimationEventHandler.OnAttack += OnAttackEnd;
        }
        public override NodeState Evaluate()
        {
            Debug.Log("Attacking");
            enemyNavMesh.SetSpeed(1f);

            if (!isWaiting && state != NodeState.RUNNING && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) < 1f)
            {
                Wait(3);
                animator.SetBool("isAttacking", true);
                state = NodeState.RUNNING;
            }
            if (!animator.GetBool("isAttacking")) state = NodeState.FAILURE;
            return state;
        }

        private void OnAttackEnd()
        {
            animator.SetBool("isAttacking", false);
            state = NodeState.FAILURE;
        }
    }
}

