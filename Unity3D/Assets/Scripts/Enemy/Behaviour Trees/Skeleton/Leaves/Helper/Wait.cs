using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Wait : Node
    {
        float duration = 1f;
        float endTime = 0f;
        public Wait(float duration): base()
        {
            this.duration = duration;
        }

        public void Escape()
        {
            state = NodeState.SUCCESS;
        }

        public override NodeState Evaluate()
        {
            switch (state)
            {
                case NodeState.FAILURE:
                    state = NodeState.RUNNING;
                    endTime = TimeMethods.GetWaitEndTime(duration);
                    break;
                case NodeState.RUNNING:
                    enemyNavMesh.SetSpeed(0f);
                    enemyNavMesh.Stop();
                    if (TimeMethods.GetWaitComplete(endTime)) state = NodeState.SUCCESS;
                    break;
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    return NodeState.SUCCESS;
            }
            return state;
        }
    }
}

