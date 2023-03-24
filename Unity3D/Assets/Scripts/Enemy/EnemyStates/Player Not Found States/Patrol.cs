using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;
using static FOV;

public class Patrol : State
{
    #region Exposed In Editor
        public override StateEnum StateEnum { get; } = StateEnum.Patrol;

        [Header("References")]
        [SerializeField] private GameObject patrolPointParentObject;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private EnemyUIManager enemyUIManager;

    [Header("Hearing Settings")]
        public float susDistance = 2f;
        public float susIntensity = 2f;
        public float susEndTime = 0f;
        public float susLength = 3f;

    #endregion Exposed In Editor

    #region Private
        private int patrolPtIdx = 0;
        protected float endTime = 0f;
        private StopZone prevStop;
        private List<Transform> patrolStops;
        private FOV fov;
        private PlayerSeenUIManager playerSeenUIManager;
        private EnemyStats enemyStats;

    #endregion Private

    protected void Start()
    {
        patrolStops = new List<Transform>();
        for (int i = 0; i < patrolPointParentObject.transform.childCount; i++)
        {
            patrolStops.Add(patrolPointParentObject.transform.GetChild(i));
        }
        fov = this.GetComponentInParent<FOV>();
        playerSeenUIManager = PlayerManager.Instance.playerSeenUIManager;
        enemyStats = GetComponentInParent<EnemyStats>();
    }
    public override void InitializeState(StateInitializationData data) 
    {
        // parent of patrol points is separate to allow separate static transform.
        patrolPtIdx = 0;
        endTime = 0f;
        prevStop = null;

        fov.ResetRouteData();
    }
    public override void ExitState() { }
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {

        enm.SetSpeed(NavMeshSpeed);
        HandlePatrolPoints(fov);

        if (TimeMethods.GetWaitComplete(susEndTime))
        {
            susEndTime = 0f;
            switch (fov.FOVStatus)
            {
                case FOVResult.Unseen:
                    enm.Patrol(GetPatrolDestination(fov));
                    break;
                case FOVResult.SusPlayer:
                    if (susEndTime == 0f)
                    {
                        susEndTime = TimeMethods.GetWaitEndTime(susLength);
                        audioManager.PlaySound("Alert", "Curious");
                        enemyUIManager.Question();
                    }
                    if (enemyStats.isAware())
                        return new StateInitializationData(StateEnum.SearchPatrol, fov.SusLocation);
                    enm.Stare(fov.SusLocation);
                    break;
                case FOVResult.SusObject:
                    enm.Stare(fov.SusLocation);
                    break;
                case FOVResult.Seen:
                    return new StateInitializationData(StateEnum.Chase, PlayerManager.Instance.gameObject);

                    // TODO: Handle object hitting enemy or being really close
            }
        }
        else enm.Stare(fov.SusLocation);

        HandlePlayerSusUI(fov);
        return new StateInitializationData(StateEnum);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        Debug.Log("Heard something");
        if (Vector3.Distance(transform.position, soundOrigin) <= susDistance && intensity >= susIntensity)
            return new StateInitializationData(StateEnum.SearchPatrol, soundOrigin);
        return new StateInitializationData(StateEnum.SearchPatrol);
    }

    protected void HandlePlayerSusUI(FOV fov)
    {
        if (fov.FOVStatus == FOVResult.SusPlayer)
        {
            float angleToEnemy = getSusAngle(fov);

            if (angleToEnemy != Mathf.Infinity)
                playerSeenUIManager.TurnOnArrow((int)angleToEnemy);
        }
        else playerSeenUIManager.TurnOffArrow();

    }
    private float getSusAngle(FOV fov)
    {
        if (fov.FOVStatus == FOVResult.SusPlayer)
        {
            Vector3 _camF = PlayerManager.Instance.camera.forward;
            Vector3 _enemyF = fov.transform.forward;

            float targetAngle = Vector3.SignedAngle(_camF, _enemyF, Vector3.up);
            
            if (targetAngle < 0) targetAngle += 360;
            return -targetAngle;
        }
        return Mathf.Infinity;
    }
    /// <summary>
    /// Iterate through patrol points when the player reaches one.
    /// </summary>
    protected void HandlePatrolPoints(FOV fov)
    {
        if (fov.PatrolPointInRange)
        {
            endTime = TimeMethods.GetWaitEndTime(fov.PreviousStop.waitTime);
            fov.PatrolPointInRange = false;
            if (patrolPtIdx + 1 >= patrolStops.Count)
                patrolPtIdx = 0;
            else
                patrolPtIdx += 1;
        }
    }
    protected Vector3 GetPatrolDestination(FOV fov)
    {
        if (fov.PreviousStop != null)
        {
            if (fov.PreviousStop != this.prevStop)
            {
                TimeMethods.GetWaitEndTime(fov.PreviousStop.waitTime);
                this.prevStop = fov.PreviousStop;
            }
        }
        if (!TimeMethods.GetWaitComplete(endTime))
            return transform.position; // wait here

        return patrolStops[patrolPtIdx].transform.position;
    }
}