using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Iterator : Node
    {
        #region Constructors
            public Iterator() : base() { }
            public Iterator(List<Node> children) : base(children) { }
            public Iterator(params Node[] children) : base(children) { }
        #endregion
        
        
        public int childIdx = 0;
        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;

            if (childIdx >= children.Count) return Exit();

            if (children[childIdx].Evaluate() != NodeState.RUNNING)
                childIdx++;

            return state;
        }

        private NodeState Exit()
        {
            state = NodeState.FAILURE;
            childIdx = 0;
            foreach (Node child in children) child.state = NodeState.FAILURE;
            return NodeState.SUCCESS;
        }
    }
}

