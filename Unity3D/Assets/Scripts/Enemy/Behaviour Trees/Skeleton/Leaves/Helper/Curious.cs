using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Curious : Node
    {
        public Curious(): base()  { }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override NodeState Evaluate()
        {
            Vector3 susLocation = (Vector3)GetData("SusLocation");
            Debug.Log("Curious");
            if (state == NodeState.RUNNING)
                enemyNavMesh.Stare(susLocation);
            if (state == NodeState.SUCCESS)
                animator.SetBool("isSurprised", false);
            if (state == NodeState.FAILURE)
            {
                if (!animator.GetBool("isSurprised"))
                {
                    enemyAnimationEventHandler.OnSurprise += OnCuriousEnd;
                    state = NodeState.RUNNING;
                }
                else state = NodeState.SUCCESS;

                animator.SetBool("isSurprised", true);
                enemyUIManager.UIanimator.SetTrigger("Question");
                audioManager.PlaySound("Alert", "Curious");
            }

            return state;
        }

        private void OnCuriousEnd()
        {
            animator.SetBool("isSurprised", false);
            enemyAnimationEventHandler.OnSurprise -= OnCuriousEnd;
            state = NodeState.SUCCESS;
        }
    }
}

