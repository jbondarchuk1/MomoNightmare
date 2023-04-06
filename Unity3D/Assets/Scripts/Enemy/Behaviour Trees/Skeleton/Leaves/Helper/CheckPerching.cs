using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviourTree
{
    public class CheckPerching : Node
    {
        bool perching = false;
        PerchData data;
        public CheckPerching(PerchData perchData) : base()
        {
            perching = perchData.perch;
            data = perchData;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Checking Perch");
            animator.SetBool("isAiming", false);
            
            Vector3 start = transform.position;
            start.y += 1;
            if (perching && Physics.Raycast(start, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerManager.GetMask(LayerManager.Layers.Interactable)))
            {
                if (hit.transform.gameObject.TryGetComponent(out DestructibleObject DO))
                {
                    if (DO.isBroken) return Exit();
                    return NodeState.SUCCESS;
                }
            }
            return Exit();
        }
        private NodeState Exit()
        {
            if (perching) enemyManager.Damage(2000);
            data.spotCone.gameObject.SetActive(false);
            return NodeState.FAILURE;
        }
    }
}
