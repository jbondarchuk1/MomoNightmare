using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace BehaviourTree
{
    public class Exclamation : Node
    {
        public bool playSurprise = true;
        private CinematicUIManager cinematicUIManager;
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        public Exclamation(CinemachineVirtualCamera data, bool surprise = true) : base() 
        { 
            this.cinemachineVirtualCamera = data;
            playSurprise = surprise;
        }

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
                if (!isWaiting || !animator.GetBool("isSurprised")) OnExclamationEnd();
                return state;
            }
            if (state == NodeState.SUCCESS)
                animator.SetBool("isSurprised", false);

            if (state == NodeState.FAILURE)
            {
                if (!animator.GetBool("isSurprised"))
                {
                    if (playSurprise)
                    {
                        enemyAnimationEventHandler.OnSurprise += OnExclamationEnd;
                        animator.SetBool("isSurprised", true);
                    }
                    state = NodeState.RUNNING;
                    Wait(3);
                }
                else state = NodeState.SUCCESS;

                if (this.cinemachineVirtualCamera != null)
                {
                    object ob = GetData("OnAlert");
                    bool wasOnAlert = ob == null ? false : (bool)ob;
                    if (!wasOnAlert) enemyManager.StartCoroutine(SetCamera(1));
                }

                enemyUIManager.UIanimator.SetTrigger("Exclamation");
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
