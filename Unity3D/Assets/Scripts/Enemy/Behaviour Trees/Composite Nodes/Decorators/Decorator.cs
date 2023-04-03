using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Decorator : Node
    {
        public Decorator(Node child):base(new List<Node> { child }) { }
        public override NodeState Evaluate()
        {
            if (!Decorate() || children.Count != 1) return NodeState.FAILURE;
            return children[0].Evaluate();
        }
        protected abstract bool Decorate();
    }
}

