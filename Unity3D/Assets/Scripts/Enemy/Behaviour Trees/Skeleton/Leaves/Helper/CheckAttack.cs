using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CheckAttack : Node
    {
        float maxAggroDistance = 20f;
        public override NodeState Evaluate()
        {
            switch (state)
            {
                case NodeState.FAILURE:
                    bool aggro = fov.FOVStatus == FOV.FOVResult.Seen;
                    if (aggro && fov.SusLocation == PlayerManager.Instance.transform.position) Debug.Log("Attack is Player");
                    if (aggro) 
                    {
                        parent.SetData("OnAlert", enemyGroupManager.OnAlert);
                        if (!enemyGroupManager.OnAlert) 
                        {
                            animator.SetTrigger("screamTrigger");
                            animator.SetBool("isSurprised", false);
                        }
                        enemyGroupManager.Alert();
                        state = NodeState.SUCCESS;
                    }
                    break;
                case NodeState.SUCCESS:
                    if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) >= maxAggroDistance)
                        state = NodeState.FAILURE;
                    break;
            }
            return state;
        }

        private void OnPlayerDeath()
        {
            state = NodeState.FAILURE;
            PlayerManager.Instance.OnDie -= OnPlayerDeath;
        }

        private void OnScreamEnd()
        {
            animator.SetBool("isScreaming", false);
        }
    }
}

