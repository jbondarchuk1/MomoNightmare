using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public abstract class PhysicalProjectileAbility : ProjectileAbility
{
    [Space]

    [Header("Settings for: ONLY PHYSICAL")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float shootForce = 150f;
    public override void EnterAbility()
    {
        _inputs.ResetActionInput();
    }
    protected Projectile ShootObject(GameObject prefab)
    {
        if (ammo > 0)
        {
            ammo--;
            endTime = GetWaitEndTime(coolDownTimer);
            GameObject shootyThing = Instantiate(prefab, ShootOrigin.position, Quaternion.Euler(0f, 0f, 0f), null);

            Rigidbody rb;
            if (shootyThing.TryGetComponent(out rb) == false)
                rb = shootyThing.AddComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            shootyThing.transform.position = ShootOrigin.position;
            rb.AddForce(Cam.forward * shootForce);

            return shootyThing.GetComponent<Projectile>();
        }
        else
        {
            Debug.Log("No ammo left");
            return null;
        }
    }
}
