using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class ProjectileAbility : MonoBehaviour
{
    public StarterAssetsInputs _inputs;
    public GameObject projectilePrefab;
    [HideInInspector]public GameObject projectile;

    public float shootForce = 150f;
    public Transform cam;
    public Transform shootOrigin;

    public bool attached = false;
    public float coolDownTimer = 0f;
    protected float endTime = 0f;

    protected GameObject ShootObject(GameObject obj)
    {
        endTime = GetWaitTime(coolDownTimer);
        GameObject shootyThing = Instantiate(obj, shootOrigin.position, Quaternion.Euler(0f, 0f, 0f), null);

        Rigidbody rb;
        if (shootyThing.TryGetComponent(out rb) == false)
            rb = shootyThing.AddComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        shootyThing.transform.position = shootOrigin.position;
        rb.AddForce(cam.forward * shootForce);

        return shootyThing;
    }

    protected GameObject RayShoot(Transform origin, LayerMask target, LayerMask obstruction)
    {
        if (Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, target | obstruction))
        {
            if ((hit.transform.gameObject.layer & target) > 0)
                return hit.transform.gameObject;
        }
        return null;
    }

    protected GameObject CapsuleRayShoot(Transform origin, LayerMask target, LayerMask obstruction)
    {
        if (Physics.CapsuleCast(origin.position, Vector3.forward, 5f, origin.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, target | obstruction))
        {
            if (1 << hit.transform.gameObject.layer == target.value) // LayerMask.NameToLayer("Enemy")))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}
