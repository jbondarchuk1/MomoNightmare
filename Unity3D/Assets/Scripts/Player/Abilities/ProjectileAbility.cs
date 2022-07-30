using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public abstract class ProjectileAbility : MonoBehaviour
{
    [Header("Required")]
    public StarterAssetsInputs _inputs;
    public Transform cam;
    public Transform shootOrigin;

    [Header("All")]
    public float ammo = Mathf.Infinity;
    public float coolDownTimer = 0f;
    [Space]

    [HideInInspector] protected float endTime = 0f;
    [HideInInspector] public bool attached = false;

    [Header("ONLY PHYSICAL")]
    public GameObject projectilePrefab;
    public float shootForce = 150f;
    [HideInInspector] public GameObject projectile; // the shot first instantiated projectile




    protected GameObject ShootObject(Projectile projectile) // , int reduceBy = 1)
    {
        if (ammo > 0)
        {
            ammo--;
            endTime = GetWaitTime(coolDownTimer);
            GameObject shootyThing = Instantiate(projectile.gameObject, shootOrigin.position, Quaternion.Euler(0f, 0f, 0f), null);

            Rigidbody rb;
            if (shootyThing.TryGetComponent(out rb) == false)
                rb = shootyThing.AddComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            shootyThing.transform.position = shootOrigin.position;
            rb.AddForce(cam.forward * shootForce);

            return shootyThing;
        }
        else
        {
            Debug.Log("No ammo left");
            return null;
        }
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
    protected GameObject RayShoot(Ray ray, LayerMask target, LayerMask obstruction)
    {
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, target | obstruction))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
            if (1 << hit.transform.gameObject.layer == target.value)
                return hit.transform.gameObject;
        }
        return null;
    }

    protected GameObject CapsuleRayShoot(float range, float radius, Ray ray, LayerMask target, LayerMask obstruction)
    {
        Vector3 point2 = ray.GetPoint(range);
        RaycastHit[] hits = Physics.CapsuleCastAll(ray.origin, point2, radius, ray.direction, range, target);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.name != "Player")
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    public void FoundAmmo()
    {
        ammo += 2f;
    }
    public void FoundAmmo(int count)
    {
        ammo += 2f * (float)count;
    }
}
