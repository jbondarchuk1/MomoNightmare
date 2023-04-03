using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// AND logical equivalent
    /// break evaluation on the first failure
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        public Sequence(params Node[] children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        break;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                }
            }
            return state;
        }
    }
}

