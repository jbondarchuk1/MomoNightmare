using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private NavMeshAgent nma;
    private GameObject AttackedObject = null;
    public Vector3 Destination { get; set; } = Vector3.zero;

    void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (AttackedObject != null && AttackedObject.gameObject.activeInHierarchy)
            Destination = AttackedObject.transform.position;
        else AttackedObject = null;
        
        if (nma.destination != Destination)
            nma.destination = Destination;
    }

    public void Chase(GameObject attackedObject)
    {
        AttackedObject = attackedObject;
    }
    public void Patrol(Vector3 location)
    {
        Destination = new Vector3(location.x, transform.position.y, location.z);
    }
    public void Patrol(Vector3 location, bool hasVerticality)
    {
        if (hasVerticality) Destination = location;
        else Patrol(location);
    }
    public void Stop()
    {
        Destination = this.transform.position;
    }
    public void Stare(Vector3 lookAt)
    {
        Destination = this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(lookAt - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, newRot.z, newRot.w);
    }
    public void SetSpeed(float speed)
    {
        nma.speed = speed;
    }
}
