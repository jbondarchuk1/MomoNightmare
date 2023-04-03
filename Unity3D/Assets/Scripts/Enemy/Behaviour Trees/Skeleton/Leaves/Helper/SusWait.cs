using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class SusWait : Wait
    {
        public SusWait(float duration) : base(duration) { }
        public override NodeState Evaluate()
        {
            Vector3 loc = ((Vector3)GetData("SusLocation"));
            if (Vector3.Distance(transform.position, loc) >= 3)
            {
                state = NodeState.SUCCESS;
                return state;
            }
            return base.Evaluate();
        }
    }
}

