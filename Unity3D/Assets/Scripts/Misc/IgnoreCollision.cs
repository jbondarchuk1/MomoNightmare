using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [HideInInspector] public Collider collider;
    protected void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        IgnoreCollisions(other);
    }
    protected void OnTriggerExit(Collider other)
    {
        IgnoreCollisions(other);
    }

    protected void IgnoreCollisions(Collider other)
    {
        if (this.collider != null)
            Physics.IgnoreCollision(other, collider, true);
    }
}
