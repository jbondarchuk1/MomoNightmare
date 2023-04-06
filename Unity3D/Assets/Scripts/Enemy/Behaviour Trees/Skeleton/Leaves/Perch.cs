using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [System.Serializable]
    public class PerchData
    {
        [SerializeField] public bool perch = false;
        [SerializeField] public float waitTime = 1f;
        [SerializeField] public SpotCone spotCone = null;
        [SerializeField] public List<float> rotations = new List<float>();
        [SerializeField] public bool horizontal = false;
    }
    public class Perch : Node
    {
        private List<float> rotations = new List<float>();
        private SpotCone cone;
        private float waitTime;
        int idx = 0;
        PerchData data;
        public Perch(PerchData data): base()
        {
            this.rotations = data.rotations;
            this.cone = data.spotCone;
            this.waitTime = data.waitTime;
            this.data = data;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Perching");
            cone.gameObject.SetActive(true);

            animator.SetBool("isAiming", true);
            enemyNavMesh.SetSpeed(0);
            enemyNavMesh.Destination = transform.position;

            float currRot = rotations[idx];
            if (currRot < 0) currRot += 360;

            if (isWaiting) return NodeState.RUNNING;
            
            if (cone.PlayerSpotted )
            {
                bool wasAlert = enemyGroupManager.OnAlert;
                enemyGroupManager.Alert();
                enemyGroupManager.checkPoint = PlayerManager.Instance.transform.position;
                if (!wasAlert)
                {
                    Wait(waitTime);
                    return NodeState.SUCCESS;
                }
            }

            Vector3 toRot = data.horizontal ? new Vector3(0,currRot,0) : new Vector3(0, 0, currRot);
            Vector3 lerpRotation = Vector3.Lerp(
                    cone.transform.parent.localRotation.eulerAngles,
                    toRot,
                    Time.deltaTime
                );

            float rot = data.horizontal ? lerpRotation.y : lerpRotation.z;
            bool exit = Mathf.Abs(rot - currRot) <= 3 || Mathf.Abs(rot - 360 - currRot) <= 3;
            cone.transform.parent.localRotation = Quaternion.Euler(lerpRotation);

            if (exit)
            {
                idx = idx + 1 >= rotations.Count ? 0 : idx + 1;
                Wait(waitTime);
            }

            return NodeState.RUNNING;
        }
    }
}

