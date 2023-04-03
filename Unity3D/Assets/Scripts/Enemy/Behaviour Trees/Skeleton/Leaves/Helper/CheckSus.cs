using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace BehaviourTree
{
    /// <summary>
    /// child[0] is Curious
    /// child[1] is search
    /// </summary>
    public class CheckSus : Node
    {
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CinematicUIManager cinematicUIManager;
        public CheckSus() : base() { }
        public CheckSus(List<Node> children) : base(children) { }
        public CheckSus(CinemachineVirtualCamera data, params Node[] children) : base(children) 
        {
            this.cinemachineVirtualCamera = data;
        }
        public override void Initialize()
        {
            base.Initialize();
            cinematicUIManager = PlayerManager.Instance.uiManager.CinematicUIManager;
        }
        public override NodeState Evaluate()
        {
            enemyNavMesh.AttackedObject = null;

            if (isSus()) state = NodeState.RUNNING;
            HandleAlert();

            if (state == NodeState.RUNNING)
                Run(GetSusLocation());
            if (state == NodeState.SUCCESS)
                Reset();
            return state;
        }
        private void Run(Vector3 susLocation)
        {
            if (susLocation != Vector3.zero)
                SetData("SusLocation", susLocation);
            state = children[0].Evaluate();
        }
        private Vector3 GetSusLocation()
        {
            Vector3 susLocation = Vector3.zero;
            if (fov.SusLocation != Vector3.zero)
                susLocation = fov.SusLocation;
            else if (enemySoundListener.soundOrigin != null && enemySoundListener.soundOrigin != Vector3.zero)
                susLocation = (Vector3)enemySoundListener.soundOrigin;
            return susLocation;
        }
        private bool isSus()
        {
                return fov.FOVStatus == FOV.FOVResult.SusPlayer
             || fov.FOVStatus == FOV.FOVResult.SusObject
             || enemySoundListener.State == EnemySoundListener.ListenState.Medium
             || enemySoundListener.State == EnemySoundListener.ListenState.Loud
             || state == NodeState.RUNNING;
        }
        private void HandleAlert()
        {
            if ((enemySoundListener.State == EnemySoundListener.ListenState.Loud || fov.FOVStatus == FOV.FOVResult.AlertObject))
            {
                if (!enemyGroupManager.OnAlert) enemyManager.StartCoroutine(SetCamera(2));
                enemyGroupManager.Alert();
            }
        }
        private void Reset()
        {
            SetData("SusLocation", null);
            state = NodeState.FAILURE;
            children[0].state = NodeState.FAILURE;
            fov.FOVStatus = FOV.FOVResult.Unseen;
            enemySoundListener.soundOrigin = null;
            enemySoundListener.State = EnemySoundListener.ListenState.None;
        }
        private IEnumerator SetCamera(float duration)
        {
            if (!this.cinemachineVirtualCamera.enabled)
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
}

