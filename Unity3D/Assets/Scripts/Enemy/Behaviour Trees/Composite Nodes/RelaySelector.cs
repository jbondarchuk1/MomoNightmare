using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// OR logic gate equivalent
    /// break evaluation on the first success
    /// </summary>
    public class RelaySelector : Node
    {
        #region Constructors
        public RelaySelector() : base() { }
        public RelaySelector(List<Node> children) : base(children) { }
        public RelaySelector(params Node[] children) : base(children) { }
        #endregion

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
