using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public bool attached = false;
    public GameObject attachedObject;
    public float lifespan = 2f;
    [HideInInspector] public float endTime = 0f;

    private void Start()
    {
        endTime = GetWaitTime(lifespan);
    }

    protected void StickToObject(Collision collision)
    {
        attached = true;
        Destroy(GetComponent<Rigidbody>());
        transform.parent = collision.collider.transform;
        transform.localPosition = Vector3.zero;
        attachedObject = collision.collider.gameObject;
    }

    public void Dissipate()
    {
        gameObject.SetActive(false);
    }
}
