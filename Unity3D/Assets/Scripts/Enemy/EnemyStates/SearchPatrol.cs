using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPatrol : State
{
    public float navMeshSpeed = 2f;
    public float pointCheckRadius = 3;
    public float[] waitTimes = new float[] { 1f, 1f, 1f, 1f };
    public float supplementalSearchRadius = 5f;

    public Vector3 checkLocation = Vector3.zero;
    private List<Vector3> supplementarySearchPoints = new List<Vector3>();
    private int searchPointIndex = 0;
    [HideInInspector] public GameObject generatedCheckPoint;

    private EnemyStateManager esm;
    private Patrol patrol;
    private float endTime = 0f;

    private void Start()
    {
        esm = GetComponentInParent<EnemyStateManager>();
        patrol = esm.patrol;
    }
    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        return searchPatrol(enm,fov);
    }

    /// <summary>
    /// Keep search patrolling if we haven't found the point we're supposed to be searching for.
    /// If we found the patrol point, head back to the normal patrol route.
    /// </summary>
    private State searchPatrol(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);
        if (checkLocation != Vector3.zero)
        {
            if (fov.susLocation != checkLocation && fov.susLocation != Vector3.zero)
            {
                checkLocation = fov.susLocation;
                supplementarySearchPoints.Clear();
            }
            // conditions for generating search points
            if (supplementarySearchPoints.Count == 0)
            {
                GenerateSearchPoints(checkLocation, supplementalSearchRadius);
            }

            // conditions for calling a quits to search patrol
            if (searchPointIndex >= supplementarySearchPoints.Count)
            {
                checkLocation = Vector3.zero;
                supplementarySearchPoints = new List<Vector3>();
                searchPointIndex = 0;
                return patrol;
            }

            // conditions for the actual search, moving one point to another and patrolling at that point or waiting
            Vector3 currentLocation = supplementarySearchPoints[searchPointIndex];
            if (Vector3.Distance(transform.position, currentLocation) <= pointCheckRadius)
            {
                endTime = searchPointIndex < waitTimes.Length ? Time.time + waitTimes[searchPointIndex]: 0f;
                searchPointIndex++;
            }
            if (Waiting())
            {
                currentLocation = transform.position;
            }
            enm.PatrolNav(currentLocation);
            checkLocation.y = transform.position.y; // we only care about the x and z axes for determining the distance
        }

        return this;
    }

    /// <summary>
    /// An enemy handles sounds like this
    ///     - do not update the location if we are following the player (only change if the player runs away far enough)
    ///     - change next state accordingly depending on the intensity and location
    /// </summary>
    public State Listen(Vector3 location, float intensity)
    {
        float susSensitivity = 0f;

        // only update location if the current one is not the player
        Collider[] objs = Physics.OverlapSphere(checkLocation, pointCheckRadius , LayerMask.GetMask("Target"));
        if (objs.Length == 0)
        {
            if (intensity > susSensitivity)
                checkLocation = location;
        }
            return this;
    }

    private bool Waiting()
    {
        if (Time.time >= endTime)
        {
            endTime = 0f;
            return false;
        }
        return true;
    }

    private void GenerateSearchPoints(Vector3 location, float radius)
    {
        supplementarySearchPoints.Add(location);
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomLocation = location;
            randomLocation.x += Random.Range(-radius, radius);
            randomLocation.z += Random.Range(-radius, radius);
            supplementarySearchPoints.Add(randomLocation);
        }
    }

    public void Reset()
    {
        checkLocation = Vector3.zero;
        searchPointIndex = 0;
        supplementarySearchPoints = new List<Vector3>();
    }

}
