using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private NavMeshAgent nma;
    public GameObject AttackedObject = null;

    bool isStopped = false;
    [field: SerializeField] public Vector3 Destination { get; set; } = Vector3.zero;

    void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (nma.isOnNavMesh) nma.destination = transform.position;
        if (!isStopped)
        {
            if (AttackedObject != null && AttackedObject.gameObject.activeInHierarchy)
                Destination = AttackedObject.transform.position;
            else AttackedObject = null;

            if (nma.destination != Destination && nma.isOnNavMesh)
                nma.destination = Destination;
        }
    }
    public void Chase(GameObject attackedObject)
    {
        FreeUpdateRot();
        AttackedObject = attackedObject;
    }
    public void Patrol(Vector3 location)
    {
        FreeUpdateRot();
        Destination = new Vector3(location.x, transform.position.y, location.z);
    }
    public void Patrol(Vector3 location, bool hasVerticality)
    {
        if (hasVerticality) Destination = location;
        else Patrol(location);
        FreeUpdateRot();
    }
    public void Stop()
    {
        FreezeUpdateRot();
        Destination = this.transform.position;
        if (!isStopped) StartCoroutine(StopTimer(1));
    }

    private IEnumerator StopTimer(float time)
    {
        isStopped = true;
        yield return new WaitForSeconds(time);
        isStopped = false;
    }
    public void Stare(Vector3 lookAt)
    {
        if (lookAt == Vector3.zero) return;

        Destination = this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(lookAt - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = Quaternion.Euler(0, newRot.eulerAngles.y, 0);
        FreezeUpdateRot();
    }
    public void SetSpeed(float speed)
    {
        nma.speed = speed;
    }
    private void FreezeUpdateRot()
    {
        nma.updateRotation = false;
    }
    private void FreeUpdateRot()
    {
        nma.updateRotation = true;
    }
}
