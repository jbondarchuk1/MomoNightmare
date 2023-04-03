using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class CheckDecorator : Decorator
    {
        private List<string> parentBoolChecks = new List<string>();

        public CheckDecorator(Node child) : base(child) { }
        public CheckDecorator(Node child, List<string> parentBoolChecks) : base(child) 
        {
            this.parentBoolChecks = parentBoolChecks;
        }

        protected override bool Decorate()
        {
            bool proceed = true;
            foreach (string check in parentBoolChecks) proceed &= (bool)GetData(check);
            return proceed;
        }
    }
}

