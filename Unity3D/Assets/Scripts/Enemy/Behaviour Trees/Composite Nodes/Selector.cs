using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// OR logic gate equivalent
    /// break evaluation on the first success
    /// </summary>
    public class Selector : Node
    {
        #region Constructors
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        public Selector(params Node[] children) : base(children) { }
        #endregion

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
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
