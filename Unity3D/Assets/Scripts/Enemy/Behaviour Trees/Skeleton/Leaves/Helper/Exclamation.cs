using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace BehaviourTree
{
    
    public class Exclamation : Node
    {
        private CinematicUIManager cinematicUIManager;
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        public Exclamation(CinemachineVirtualCamera data) : base() { this.cinemachineVirtualCamera = data; }

        public override void Initialize()
        {
            base.Initialize();
            cinematicUIManager = PlayerManager.Instance.uiManager.CinematicUIManager;
        }
        public override NodeState Evaluate()
        {
            Debug.Log("Exclamation");
            if (state == NodeState.RUNNING)
            {
                enemyNavMesh.Stop();
                enemyNavMesh.Stare(fov.SusLocation);
            }
            if (state == NodeState.SUCCESS)
                animator.SetBool("isSurprised", false);
            if (state == NodeState.FAILURE)
            {
                if (!animator.GetBool("isSurprised"))
                {
                    enemyAnimationEventHandler.OnSurprise += OnExclamationEnd;
                    state = NodeState.RUNNING;
                }
                else state = NodeState.SUCCESS;
                
                
                if (this.cinemachineVirtualCamera != null)
                {
                    enemyManager.StartCoroutine(SetCamera(1));
                }
                animator.SetBool("isSurprised", true);
                enemyUIManager.UIanimator.SetTrigger("Exclamation");
                // audioManager.PlaySound("Alert", "Growl");
            }
            return state;
        }

        private void OnExclamationEnd()
        {
            animator.SetBool("isSurprised", false);
            enemyAnimationEventHandler.OnSurprise -= OnExclamationEnd;
            state = NodeState.SUCCESS;
        }

        private IEnumerator SetCamera(float duration)
        {
            this.cinemachineVirtualCamera.enabled = true;
            cinematicUIManager.Activate();
            PlayerManager.Instance.playerMovementManager.canMove = false;
            yield return new WaitForSeconds(duration);
            this.cinemachineVirtualCamera.enabled = false;
            cinematicUIManager.Deactivate();
            PlayerManager.Instance.playerMovementManager.canMove = true;
        }
    }
}
