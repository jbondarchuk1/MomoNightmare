using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Alert : Patrol
    {
        private float checkSpotDistance = 20f;
        private Vector3? groupCheckPoint;

        public Alert(PatrolValues patrolValues, float checkSpotDistance): base(patrolValues)
        {
            this.checkSpotDistance = checkSpotDistance;
        }
        public override NodeState Evaluate()
        {
            NodeState ns = base.Evaluate();

            if (enemyGroupManager.checkPoint != null)
            {
                float distance = Vector3.Distance(transform.position, (Vector3)enemyGroupManager.checkPoint);
                if (distance < checkSpotDistance)
                {
                    groupCheckPoint = enemyGroupManager.checkPoint;
                    enemyGroupManager.checkPoint = null;
                }
            }
            if (groupCheckPoint == null) return ns;


            if (enemyGroupManager.checkPoint == null && groupCheckPoint == null) return ns;

            enemyNavMesh.Destination = (Vector3)groupCheckPoint;
            float dist = Vector3.Distance(transform.position, (Vector3)groupCheckPoint);
            if (dist < 3) groupCheckPoint = null;
            
            return ns;
        }
    }
}