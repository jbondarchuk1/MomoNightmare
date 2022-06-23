using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public Collider collider;
    private void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, collider, true);
    }
    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, collider, true);
    }
}
