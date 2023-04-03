using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyGroupManager;

namespace BehaviourTree
{
    public class CheckAlert : Node
    {
        public override NodeState Evaluate()
        {
            if (enemyGroupManager.OnAlert)
                return NodeState.SUCCESS;
            return NodeState.FAILURE;
        }

    }
}
