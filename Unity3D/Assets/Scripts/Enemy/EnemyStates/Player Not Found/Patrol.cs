using Assets.Scripts;
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

        [Header("General Settings")]
        public float navMeshSpeed = 2f;

        [Header("References")]
        public GameObject patrolPointParentObject;

        [Header("Hearing Settings")]
        public float susDistance = 2f;
        public float susIntensity = 2f;

    #endregion Exposed In Editor

    #region Private
        private int patrolPtIdx = 0;
        protected float endTime = 0f;
        private Collider prevStop;
        private List<Transform> patrolStops;
        private FOV fov;
    #endregion Private

    protected void Start()
    {
        patrolStops = new List<Transform>();
        for (int i = 0; i < patrolPointParentObject.transform.childCount; i++)
        {
            patrolStops.Add(patrolPointParentObject.transform.GetChild(i));
        }
        fov = this.GetComponentInParent<FOV>();
    }
    public override void InitializeState(StateInitializationData data) 
    {
        // parent of patrol points is separate to allow separate static transform.
        patrolPtIdx = 0;
        endTime = 0f;
        prevStop = null;


        fov.ResetRouteData();
    }

    // TODO I think this needs general rework
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);
        HandlePatrolPoints(fov);
        switch (fov.FOVStatus)
        {
            case FOVResult.Unseen:
                enm.Patrol(GetPatrolDestination(fov));
                break;
            case FOVResult.SusPlayer:
                enm.Stare(fov.SusLocation);
                break;
            case FOVResult.SusObject:
                enm.Stare(fov.SusLocation);
                break;
            case FOVResult.Seen:
                return new StateInitializationData(StateEnum.Chase, GameObject.Find("Player"));

                // TODO: Handle object hitting enemy or being really close
        }
        return new StateInitializationData(StateEnum);
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        if (Vector3.Distance(transform.position, soundOrigin) <= susDistance && intensity >= susIntensity)
            return new StateInitializationData(StateEnum.SearchPatrol, soundOrigin);
        return new StateInitializationData(StateEnum.SearchPatrol);
    }
    public override void ExitState() { }


    /// <summary>
    /// Iterate through patrol points when the player reaches one.
    /// </summary>
    protected void HandlePatrolPoints(FOV fov)
    {
        if (fov.PatrolPointInRange)
        {
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
                TimeMethods.GetWaitEndTime(fov.PreviousStop.gameObject.GetComponent<StopZone>().waitTime);
                this.prevStop = fov.PreviousStop;
            }
        }
        if (!TimeMethods.GetWaitComplete(endTime))
            return transform.position; // wait here

        return patrolStops[patrolPtIdx].transform.position;
    }

}