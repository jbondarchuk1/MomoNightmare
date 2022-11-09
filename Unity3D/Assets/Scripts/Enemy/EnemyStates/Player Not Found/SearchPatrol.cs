using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

public class SearchPatrol : State
{
    public override StateEnum StateEnum { get; } = StateEnum.SearchPatrol;

    #region Exposed In Editor

        [Header("Settings")]
        [SerializeField] private float pointCheckRadius = 3;
        [SerializeField] private float supplementalSearchRadius = 5f;

        [Header("Temporary")]
        [SerializeField] private float[] waitTimes = new float[] { 1f, 1f, 1f, 1f };

    #endregion Exposed In Editor

    #region Private

        private Vector3 checkLocation = Vector3.zero;
        private List<Vector3> searchPoints = new List<Vector3>();
        private int searchPtIdx = 0;
        private float endTime = 0f;

    #endregion Private

    #region Public Methods

    public override void InitializeState(StateInitializationData data)
    {
        Reset();
        checkLocation = data.Location;
    }
    public override void ExitState()
    {
        Reset();
    }
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(NavMeshSpeed);
        if (searchPoints.Count == 0) GenerateSearchPoints();
        return new StateInitializationData(Search(enm));
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    public void Reset()
    {
        checkLocation = Vector3.zero;
        searchPtIdx = 0;
        searchPoints = new List<Vector3>();
    }
    public void OverrideSearchLocation(Vector3 location)
    {
        Reset();
        checkLocation = location;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Keep search patrolling if we haven't found the point we're supposed to be searching for.
    /// If we found the patrol point, head back to the normal patrol route.
    /// </summary>
    private StateEnum Search(EnemyNavMesh enm)
    {
        // Error or Normal Exit state conditions
        if (checkLocation == Vector3.zero || searchPtIdx >= searchPoints.Count) 
            return StateEnum.Patrol;


        // ITERATE THROUGH SEARCH POINTS
        Vector3 currSearchPoint = searchPoints[searchPtIdx];
        if (Vector3.Distance(transform.position, currSearchPoint) <= pointCheckRadius)
        {
            endTime = searchPtIdx < waitTimes.Length ? Time.time + waitTimes[searchPtIdx]: 0f;
            searchPtIdx++;
        }

        // WAIT
        if (!TimeMethods.GetWaitComplete(endTime)) currSearchPoint = transform.position;
        
        // NAVIGATE TO POINT
        enm.Patrol(currSearchPoint);
        checkLocation.y = transform.position.y; // we only care about the x and z axes for determining the distance

        return StateEnum;
    }

    private void GenerateSearchPoints()
    {
        searchPoints.Add(checkLocation);
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomLocation = checkLocation;
            randomLocation.x += Random.Range(-pointCheckRadius, pointCheckRadius);
            randomLocation.z += Random.Range(-pointCheckRadius, pointCheckRadius);
            searchPoints.Add(randomLocation);
        }
    }

    #endregion Private Methods
}
