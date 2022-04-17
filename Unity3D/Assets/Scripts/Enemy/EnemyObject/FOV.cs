using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for checking what objects are in range of the game object. 
/// *SOUNDS ARE NOT HANDLED HERE*
/// </summary>
public class FOV : MonoBehaviour
{
    [HideInInspector] public bool patrolling;
    [HideInInspector] public GameObject playerRef;
    [HideInInspector] public Collider previousStop;
    public Transform eyes;

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

    public  LayerMask obstructionMask;
    private LayerMask targetMask;

    [Header("FOV Senses")]
        public bool canSeePlayer;
        public bool playerInRange;
        public bool patrolPointInRange;
        public bool sus;
        public Vector3 susLocation = Vector3.zero;
        public bool targetNotPlayer = false; 
        public Collider[] nearbyColliders = null;

    private void Start()
    {
        playerRef = GameObject.Find("Player");
        currFOVValues = fovValues.Count > 0 ? fovValues[0] : new FOVValues();
    }
    private void Update()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        currFOVValues = fovValues[currFOVIdx];

        if (patrolling)
        {
            // Sense Patrol Routes
            targetMask = LayerMask.GetMask("Routes");
            FOVCheck();
        }

        // Sense Player
        targetMask = LayerMask.GetMask("Target");
        FOVCheck();
        yield return wait;
    }

    private void FOVCheck()
    {
        Collider[] obj;
        switch (targetMask.value)
        {
            case 1 << 6:
                obj = Physics.OverlapSphere(transform.position, currFOVValues.radius, targetMask);
                CheckForPlayer(obj);
                nearbyColliders = obj;
                break;
            case 1 << 11:
                obj = Physics.OverlapSphere(transform.position, currFOVValues.visitRadius, targetMask);
                patrolPointInRange = patrolPointInRange == false ? CheckForPatrolPoint(obj) : patrolPointInRange;
                break;
        }
    }

    private bool CheckForPatrolPoint(Collider[] obj)
    {
        if (obj.Length > 0)
        {
            if (previousStop == null || obj[0].name != previousStop.name)
            {
                previousStop = obj[0];
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// In inner circle -> In view cone angles == player seen
    /// in outer circle -> in view cone angles == sus
    /// else: unseen
    /// 
    /// Sounds handled in the state machine
    /// </summary>
    /// <param name="obj"></param>
    private void CheckForPlayer(Collider[] obj)
    {
        if (obj.Length > 0)
        {
            targetNotPlayer = true;
            foreach(Collider gameObject in obj)
            {
                if (gameObject.gameObject.name.Contains("Player"))
                {
                    targetNotPlayer = false;
                }
            }
            
            
            playerInRange = true;

            Transform target = obj[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // CHECK PLAYER IN IMMEDIATE VIEW
            if (angleToTarget < currFOVValues.angle / 2)
            {
                float distance = Vector3.Distance(eyes.position, target.position);
                bool obstructed = !Physics.Raycast(eyes.position, directionToTarget, distance, obstructionMask.value);
                if (obstructed)
                {
                    if (distance <= currFOVValues.innerRadius) // INNER RADIUS IS IMMEDIATE SPOT
                    {
                        canSeePlayer = true;
                    }
                    else
                    {
                        // OUTER RADIUS SPOTTED IS SUS 
                        sus = true;
                        susLocation = target.position;
                    }
                }
                else
                {
                    canSeePlayer = false;
                    sus = false;
                    susLocation = Vector3.zero;
                }
            }
            // CHECK PLAYER IN REAR VIEW
            else if (angleToTarget > currFOVValues.rearOuterAngle && angleToTarget < currFOVValues.rearInnerAngle)
            {
                if (Vector3.Distance(transform.position, target.position) < currFOVValues.maxRearDistance)
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
            {
                canSeePlayer = false;
            }

            if (canSeePlayer)
            {
                sus = false;
            }
        }
        else
        {
            sus = false;
            canSeePlayer = false;
            playerInRange = false;
        }
    }
}