using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;


public abstract class Projectile : MonoBehaviour
{
    public bool attached = false;
    public GameObject attachedObject;
    public float lifespan = 2f;
    [HideInInspector] public float endTime = 0f;
    int originalLayer;

    private void Start()
    {
        endTime = GetWaitEndTime(lifespan);
        originalLayer = this.gameObject.layer;
    }

    protected void StickToObject(Collision collision)
    {
        Debug.Log("Stick to " + collision.gameObject.name);
        attached = true;
        if (TryGetComponent(out Rigidbody body)) body.isKinematic = true;

        transform.parent = collision.collider.transform;
        attachedObject = collision.collider.gameObject;
    }

    public void DeleteProjectile()
    {
        Debug.Log("DELETING PROJECTILE");
        if (TryGetComponent(out Rigidbody body)) body.isKinematic = false;

        gameObject.layer = originalLayer;
        transform.parent = null;
        attachedObject = null;
        attached = false;
        this.gameObject.SetActive(false);
    }

    public abstract void ActivateProjectile();
}
