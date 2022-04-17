using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopZone : MonoBehaviour
{
    public BoxCollider collider;
    public bool destructable = false;
    public float waitTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(collider, other);
        if (other.name.Contains("Enemy")) HandleDestroy();
    }

    public void HandleDestroy()
    {
        if (destructable)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
