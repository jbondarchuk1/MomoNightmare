using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public abstract class ProjectileAbility : AbilityBase
{
    #region Exposed In Editor
    [Header("Required References")]
    [SerializeField] protected Transform cam;
    [SerializeField] protected Transform shootOrigin;
    [Space]

    [Header("Settings for: All Projectiles")]
    [SerializeField] protected float ammo = Mathf.Infinity;
    [SerializeField] protected float coolDownTimer = 0f;
    [SerializeField] protected float hitTimer = 0f;
    [Space]

    [Header("Settings for: ONLY PHYSICAL")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float shootForce = 150f;
    #endregion Exposed In Editor

    protected StarterAssetsInputs _inputs;

    protected void Start()
    {
        _inputs = StarterAssetsInputs.Instance;
    }

    #region Hidden In Editor
    [HideInInspector] protected float endTime = 0f;
    #endregion Hidden In Editor
    public void PickUpAmmo(int count = 1)
    {
        ammo += 2f * (float)count;
    }
    protected Projectile ShootObject(GameObject prefab)
    {
        if (ammo > 0)
        {
            ammo--;
            endTime = GetWaitEndTime(coolDownTimer);
            GameObject shootyThing = Instantiate(prefab.gameObject, shootOrigin.position, Quaternion.Euler(0f, 0f, 0f), null);

            Rigidbody rb;
            if (shootyThing.TryGetComponent(out rb) == false)
                rb = shootyThing.AddComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            shootyThing.transform.position = shootOrigin.position;
            rb.AddForce(cam.forward * shootForce);

            return shootyThing.GetComponent<Projectile>();
        }
        else
        {
            Debug.Log("No ammo left");
            return null;
        }
    }
    protected GameObject ShootRay(Transform origin, LayerMask target, LayerMask obstruction)
    {
        Ray ray = new Ray(origin.position, origin.TransformDirection(Vector3.forward));
        return ShootRay(ray, target, obstruction);
    }
    protected GameObject ShootRay(Ray ray, LayerMask target, LayerMask obstruction)
    {
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, target | obstruction))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
            if (1 << hit.transform.gameObject.layer == target.value)
                return hit.transform.gameObject;
        }
        return null;
    }
    protected GameObject ShootCapsuleRay(float range, float radius, Ray ray, LayerMask target, LayerMask obstruction)
    {
        Vector3 point2 = ray.GetPoint(range);

        RaycastHit[] hits = Physics.CapsuleCastAll(ray.origin, point2, radius, ray.direction, range, target);
        foreach (RaycastHit hit in hits)
        {
            LayerMask hitLayerMask = LayerMask.GetMask(LayerMask.LayerToName(hit.transform.gameObject.layer));
            LayerMask hitObstructedLayers = hitLayerMask & obstruction;
            if (hitObstructedLayers == 0) return hit.transform.gameObject;
        }
        return null;
    }
}
