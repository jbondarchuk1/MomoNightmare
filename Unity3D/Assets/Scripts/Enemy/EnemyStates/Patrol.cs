using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    public float navMeshSpeed = 2f;
    
    public EnemyStats stats;
    protected EnemyStateManager esm;
    protected EnemyNavMesh enm;
    protected FOV fov;

    protected SearchPatrol searchPatrol;
    public List<Transform> patrolStops;

    // parent of patrol points is separate to allow separate static transform.
    public GameObject patrolParent;
    protected int patrolIndex = 0;
    protected float endTime = 0f;
    protected Collider prevStop;

    protected Vector3 susLocation = Vector3.zero;
    protected int susLevel = -1;

    protected void Start()
    {
        esm = GetComponentInParent<EnemyStateManager>();
        searchPatrol = esm.searchPatrol;
        patrolStops = new List<Transform>();
        for (int i = 0; i < patrolParent.transform.childCount; i++)
        {
            patrolStops.Add(patrolParent.transform.GetChild(i));
        }
    }

    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);
        this.enm = enm;
        this.fov = fov;
        fov.patrolling = true;
        HandlePatrolPoints();

        switch (HandleSus())
        {
            case -1:
                enm.PatrolNav(HandleWait());
                break;
            case 0:
                enm.StopNav(fov.susLocation);
                break;
            case 1:
                if (endTime == 0f)
                {
                    endTime = Time.time + 1f;
                }
                if (HandleWait(susLocation) == susLocation)
                {
                    SetPatrol();
                    searchPatrol.checkLocation = susLocation;
                    if (searchPatrol.checkLocation == Vector3.zero)
                        Debug.Log("CheckLocation is Null, fix required");
                    return searchPatrol;
                }
                else
                {
                    enm.StopNav(susLocation);
                }
                break;
        }

        return this;
    }
    
    /// <summary>
    /// Iterate through patrol points when the player reaches one.
    /// </summary>
    protected void HandlePatrolPoints()
    {
        if (fov.patrolPointInRange)
        {
            fov.patrolPointInRange = false;
            if (patrolIndex + 1 >= patrolStops.Count)
                patrolIndex = 0;
            else
                patrolIndex += 1;
        }
    }

    /// <summary>
    /// Check the FOV if the player is in sus range. Keep in mind the player can be sussed from other stimuli, this is not complete.
    /// -1 = standard patrol
    ///  0 = player in viewcone or light tap from object
    ///  1 = hard tap from object
    /// </summary>
    protected int HandleSus()
    {
        int lvl = -1;
        if (susLocation != Vector3.zero)
        {
            lvl = susLevel;
            susLevel = -1;
        }
        else if (fov.sus)
        {
            lvl = 0;
        }
        return lvl;
    }

    /// <summary>
    /// Local method for handling a force of collision. Throws error if incorrect data is supplied.
    /// </summary>
    /// <param name="force"></param>
    /// <param name="magnitudeClamps"></param>
    /// <returns></returns>
    protected int CollisionDetected(Vector3 force, float[][] magnitudeClamps)
    {
        try
        {
            for (int i = 0; i < magnitudeClamps.Length; i++)
            {
                float[] clamps = magnitudeClamps[i];
                float minClamp = clamps[0];
                float maxClamp = clamps[1];

                if (UnityHelperMethods.ClampVector3(force, minClamp, maxClamp))
                    return i;
            }
            return -1;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return -1;
        }
    }

    /// <summary>
    /// A collision needs to be clamped to different force values. Small values might correlate with less awareness,
    /// more force would make an enemy respond more strongly. 
    /// </summary>
    /// <param name="velocity">The vector of force on impact of an object</param>
    /// <param name="magnitudeClamps">Every float array in the 2d array needs 2 values : minimumClamp and maximumClamp</param>
    /// <returns></returns>
    public void HandleCollision(Vector3 velocity, float[][] magnitudeClamps)
    {
        int collisionLevel = CollisionDetected(velocity, magnitudeClamps);
        switch (collisionLevel)
        {
            case 0:
                // light: only look at the susDirection for a second
                susLocation = new Vector3(velocity.x, 0f, velocity.z);
                susLevel = 0;
                break;

            case 1:
                // hard: look at susDirection, then search it
                susLocation = new Vector3(velocity.x, 0f, velocity.z);
                susLevel = 1;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Prepare to resume normal patrol patterns from any state that calls this function.
    /// </summary>
    public void SetPatrol()
    {
        // parent of patrol points is separate to allow separate static transform.
        patrolIndex = 0;
        endTime = 0f;
        prevStop = null;

        susLocation = Vector3.zero;

        enm.PatrolNav(patrolStops[patrolIndex].transform.position);
        if (fov != null)
        {
            fov.previousStop = null;
            fov.patrolling = true;
            fov.patrolPointInRange = false;
        }
        else
        {
            Debug.Log("No FOV");
        }

    }


    // TODO: HANDLE SOUND SENSITIVITY AND RANGE
    /// <summary>
    /// When a patrolling enemy hears a sound, switch states depending on the threshold, else stay put.
    /// </summary>
    public State Listen(Vector3 location, float intensity)
    {
        float susSensitivity = 0f; 
        if (intensity > susSensitivity)
        {
            searchPatrol.Reset();
            return searchPatrol;
        }
        return this;
    }

    protected Vector3 HandleWait()
    {
        if (fov.previousStop != null)
        {
            if (fov.previousStop != this.prevStop)
            {
                SetWaitAtZone(fov.previousStop.gameObject.GetComponent<StopZone>().waitTime);
                this.prevStop = fov.previousStop;
            }
        }
        if (Waiting())
        {
            return transform.position; // wait here
        }

        return patrolStops[patrolIndex].transform.position;
    }
    protected Vector3 HandleWait(Vector3 stop)
    {
        if (Waiting())
        {
            return transform.position;
        }
        return stop;
    }

    protected void SetWaitAtZone(float seconds)
    {
        this.endTime = Time.time + seconds;
    }

    protected bool Waiting()
    {
        if (Time.time >= endTime)
        {
            endTime = 0f;
            return false;
        }
        return true;
    }
    
}