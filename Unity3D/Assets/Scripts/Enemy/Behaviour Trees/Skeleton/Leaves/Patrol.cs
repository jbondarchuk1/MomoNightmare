using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [System.Serializable]
    public class PatrolValues : NodeValues
    {
        [SerializeField] public float speed = 1f;
        [SerializeField] public float patrolPointCheckRange = 2f;
        [SerializeField] public int fovIdx = 0;
        [SerializeField] public List<StopZone> patrolStops = new List<StopZone>();
    }

    public class Patrol : Node
    {
        private PatrolValues PatrolValues = new PatrolValues();

        #region Constructors
            public Patrol(PatrolValues patrolValues) : base() 
            { 
                PatrolValues = patrolValues;
            }
            public Patrol(List<Node> children) : base(children) { }
            public Patrol(params Node[] children) : base(children) { }
        #endregion
        
        private int patrolPtIdx = 0;

        public override NodeState Evaluate()
        {
            enemyNavMesh.Chase(null);
            fov.currFOVIdx = PatrolValues.fovIdx;
            enemyNavMesh.SetSpeed(PatrolValues.speed);
            HandlePatrolPoints();
            enemyNavMesh.Destination = GetPatrolDestination();
            return NodeState.RUNNING;
        }

        /// <summary>
        /// Iterate through patrol points when the player reaches one.
        /// </summary>
        protected void HandlePatrolPoints()
        {
            if (PatrolPointInRange() && !isWaiting)
            {
                Wait(PatrolValues.patrolStops[patrolPtIdx].waitTime);
                if (patrolPtIdx + 1 >= PatrolValues.patrolStops.Count) patrolPtIdx = 0;
                else patrolPtIdx += 1;
            }
        }
        protected bool PatrolPointInRange() =>  Vector3.Distance(transform.position, PatrolValues.patrolStops[patrolPtIdx].transform.position) <= PatrolValues.patrolPointCheckRange;
        protected Vector3 GetPatrolDestination()
        {
            if (isWaiting) return transform.position;
            return PatrolValues.patrolStops[patrolPtIdx].transform.position;
        }
    }
}
