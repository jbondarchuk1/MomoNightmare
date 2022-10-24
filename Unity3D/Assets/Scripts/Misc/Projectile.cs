using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;


public abstract class Projectile : MonoBehaviour
{

    [HideInInspector] public bool attached = false;
    public GameObject attachedObject;
    public float lifespan = 2f;
    [HideInInspector] public float endTime = 0f;

    private void Start()
    {
        endTime = GetWaitEndTime(lifespan);
    }

    protected void StickToObject(Collision collision)
    {
        attached = true;
        if (TryGetComponent(out Rigidbody body)) Destroy(body);

        transform.parent = collision.collider.transform;
        transform.localPosition = Vector3.zero;
        attachedObject = collision.collider.gameObject;
    }

    public void DeleteProjectile()
    {
        GameObject.Destroy(this);
    }

    public abstract void ActivateProjectile();
}
