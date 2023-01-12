using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

/// <summary>
/// Responsible for checking what objects are in range of the game object. 
/// *SOUNDS ARE NOT HANDLED HERE*
/// </summary>
public class FOV : MonoBehaviour
{
    public FOVResult FOVStatus = FOVResult.Unseen;
    public bool PatrolPointInRange { get; set; } = false;
    public Vector3 SusLocation { get; set; } = Vector3.zero;
    

    #region Enumerations
    public enum FOVResult { Unseen, SusPlayer, SusObject, Seen}
    public enum SpotLocation { FrontOuterRadius, FrontInnerRadius, RearInnerRadius, Unspotted }
    #endregion Enumerations

    #region Public
    [Header("Initialization Values")]
    public Transform eyes;
    public LayerMask obstructionMask;
    public StopZone PreviousStop { get; private set; }

    #endregion Public

    #region FOV Values
    [Header("FOV Circle Values")]
    public List<FOVValues> fovValues = new List<FOVValues>() { };
    [HideInInspector]public int currFOVIdx { get; set; } = 0;
    public FOVValues currFOVValues { get; set; }

    [System.Serializable]
    public class FOVValues
        {
            public string name = "";
            public float radius = 0f; // outer radius = buildup
            public float innerRadius = 0f; // inner radius = instant spot
            public float visitRadius = 0f; // visit patrol point radius

            [Range(0, 360)] public float angle = 0f;
            [Range(0, 360)] public float rearOuterAngle = 0f;
            [Range(0, 360)] public float rearInnerAngle = 0f;
            public float maxRearDistance = 0f;
        }
    #endregion FOV Values

    #region Private
    private LayerMask playerMask;
    private LayerMask routesMask;
    #endregion Private

    #region Public Methods

    public void ResetRouteData()
    {
        PreviousStop = null;
        PatrolPointInRange = false;
    }

    #endregion Public Methods

    #region Private Methods
    private void Start()
    {
        currFOVValues = fovValues.Count > 0 ? fovValues[0] : new FOVValues();
        playerMask = LayerMask.GetMask("Target");
        routesMask = LayerMask.GetMask("Routes");
        obstructionMask = LayerMask.GetMask("Ground", "Obstruction");
    }
    private void Update()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        currFOVValues = fovValues[currFOVIdx];
        FOVCheck();

        yield return wait;
    }

    /// <summary>
    /// Check within the FOV for the player and patrol point. 
    /// When patrol point is in range, wait for patrol to flip the boolean (AKA do nothing)
    /// </summary>
    private void FOVCheck()
    {
        Collider[] obj;

        // check for player
        obj = Physics.OverlapSphere(transform.position, currFOVValues.radius, playerMask);
        FOVStatus = CheckForPlayer(obj);


        // check for patrol point
        obj = Physics.OverlapSphere(transform.position, currFOVValues.visitRadius, routesMask);
        PatrolPointInRange = !PatrolPointInRange? CheckForPatrolPoint(obj) : PatrolPointInRange;
    }

    private bool CheckForPatrolPoint(Collider[] obj)
    {
        if (obj.Length > 0)
        {
            if (PreviousStop == null || obj[0].gameObject.name != PreviousStop.gameObject.name)
            {
               if (obj[0].TryGetComponent(out StopZone zone))
                    PreviousStop = zone;
                
                return true;
            }
        }
        return false;
    }
    private bool TargetIsPlayer(Collider[] obj)
    {
        if (obj.Length == 0)
            return false;

        foreach (Collider c in obj)
            if (c.gameObject.name.Contains("Player") || c.gameObject.layer == GetLayer(Layers.Target))
                return true;

        return false;
    }
    private bool PlayerInRange(Collider[] obj)
    {
        foreach (Collider gameObject in obj)
            if (gameObject.gameObject.name.Contains("Player")) return true;
        return false;
    }

    /// <summary>
    /// In inner circle -> In view cone angles == player seen
    /// In outer circle -> in view cone angles == sus
    /// else: unseen
    /// 
    /// Sounds handled in the state machine
    /// </summary>
    /// <param name="obj"></param>
    private FOVResult CheckForPlayer(Collider[] obj)
    {
        SpotLocation spotLoc = SpotLocation.Unspotted;
        bool targetIsPlayer = TargetIsPlayer(obj);
        bool playerInRange = PlayerInRange(obj);

        if (obj.Length > 0)
        {
            Transform target = obj[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // CHECK PLAYER IN FRONT VIEW
            if (angleToTarget < currFOVValues.angle / 2)
            {
                float distance = Vector3.Distance(eyes.position, target.position);
                bool obstructed = Physics.Raycast(eyes.position, directionToTarget, distance, obstructionMask.value);
                if (!obstructed)
                {
                    if (distance <= currFOVValues.innerRadius) // INNER RADIUS IS IMMEDIATE SPOT
                        spotLoc = SpotLocation.FrontInnerRadius;
                    else
                        spotLoc = SpotLocation.FrontOuterRadius; // OUTER RADIUS SPOTTED IS SUS 
                }
                else spotLoc = SpotLocation.Unspotted;
            }

            // CHECK PLAYER IN REAR VIEW
            else if (angleToTarget > currFOVValues.rearOuterAngle && angleToTarget < currFOVValues.rearInnerAngle)
            {
                if (Vector3.Distance(transform.position, target.position) < currFOVValues.maxRearDistance)
                    spotLoc = SpotLocation.RearInnerRadius;
            }
        }

        FOVResult res = FindFOVResult(spotLoc, targetIsPlayer, playerInRange);
        //if (res == FOVResult.SusPlayer || res == FOVResult.Seen) Debug.Log("SEEN");
        SusLocation = FindSusLocation(obj, res);
        return res;
    }
    
    /// <summary>
    /// Answers the question of what did we see in the FOV? Was it the player? If so are we sus or aggro?
    /// If it's not the player, do we sus it? 
    /// 
    /// Rear 6th sense only picks up on player.
    /// </summary>
    /// <param name="spotLocation"></param>
    /// <param name="targetIsPlayer"></param>
    /// <param name="playerInRange"></param>
    /// <returns></returns>
    private FOVResult FindFOVResult(SpotLocation spotLocation, bool targetIsPlayer, bool playerInRange)
    {
        FOVResult fovResult = FOVResult.Unseen;

        switch (spotLocation)
        {
            /*
             TODO: How do we handle the player throwing objects too close to enemies?
            Enemy should be alerted if an object hits them directly or gets way too close and is floating.
             */
            case SpotLocation.FrontInnerRadius:
                if (targetIsPlayer) fovResult = FOVResult.Seen;
                else if (!playerInRange) fovResult = FOVResult.SusObject;
                break;

            case SpotLocation.FrontOuterRadius:
                fovResult = targetIsPlayer ? FOVResult.SusPlayer: FOVResult.SusObject;
                break;

            case SpotLocation.RearInnerRadius:
                if (targetIsPlayer) fovResult = FOVResult.Seen;
                break;
            default: break;
        }

        Debug.Log("FOV Result: " + fovResult);
        return fovResult;

    }

    private Vector3 FindSusLocation(Collider[] obj, FOVResult fovResult)
    {
        if (obj.Length == 0) return Vector3.zero;

        Transform target = obj[0].transform;
        if (fovResult == FOVResult.SusPlayer || fovResult == FOVResult.SusObject)
        {
            return target.position;
        }
        return Vector3.zero;

    }

    #endregion Private Methods
}