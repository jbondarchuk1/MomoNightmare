using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [HideInInspector]
    public bool hitboxActive = false;
    private GameObject playerRef;
    private NavMeshAgent nma;
    private Stats opponentStats; // JUST FOR DAMAGE, THIS IS TEMPORARY
    

    public Vector3 destination;

    void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        playerRef = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        nma.destination = destination;
        // destination = Vector3.zero;
    }

    // TEMPORARY DAMAGE IS TIED TO NAVMESH, LATER WILL BE MOVED TO APPROPRIATE WEAPON
    void OnCollisionEnter(Collision collision)
    {
        Collider opponent = collision.collider;
        if (opponent.name == "Player")
        {
            opponentStats = playerRef.GetComponent<PlayerStats>();
            Damage();
        }
    }

    public void AttackNav()
    {
        destination = playerRef.transform.position;
    }
    public void PatrolNav(Vector3 location)
    {
        
        destination = new Vector3(location.x, transform.position.y, location.z);
    }
    public void PatrolNav(Vector3 location, bool involveVertical)
    {
        if (involveVertical)
            destination = location;
        else
        {
            PatrolNav(location);
        }
    }
    public void StopNav()
    {
        destination = this.transform.position;
    }
    public void StopNav(Vector3 lookAt)
    {
        destination = this.transform.position;
        transform.LookAt(lookAt);
    }
    public void SetSpeed(float speed)
    {
        nma.speed = speed;
        
    } 

    // MAY NEED TO MOVE TO A NEW SCRIPT
    void Damage()
    {
        if (!hitboxActive || opponentStats == null)
            return;

        opponentStats.health -= 10;
        if (opponentStats.health <= 0)
        {
            opponentStats.HandleDeath();
        }
    }

}
