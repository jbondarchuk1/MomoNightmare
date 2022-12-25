using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;
using static LayerManager;

public abstract class PhysicalProjectileAbility : ProjectileAbility, IPoolUser
{
    
    [Space]

    [Header("Settings for: ONLY PHYSICAL")]
    [SerializeField] protected float shootForce = 150f;
    [SerializeField] protected LayerMask hitMask; 


    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "";
    public string ProjectilePoolTag = "";
    public string WandEffectPoolTag = "";

    protected new void Start()
    {
        base.Start();
        if (hitMask == 0)
            hitMask = GetMask(new Layers[] {
        Layers.Obstruction, Layers.Default, Layers.Enemy,
        Layers.Ground, Layers.Interactable
    });
        Tag = ProjectilePoolTag;
        ObjectPooler = ObjectPooler.Instance;

    }

    public override void EnterAbility()
    {
        _inputs.ResetActionInput();
    }
    protected Projectile ShootObject()
    {
        Cam = PlayerManager.Instance.camera;
        if (ammo > 0)
        {
            Ray ray = new Ray(Cam.position, Cam.forward);

            Vector3 aimLoc = ray.GetPoint(5);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, hitMask))
            {
                Debug.Log("Hit something");
                aimLoc = hit.point;
            }

            Vector3 aimDirection = (aimLoc - ShootOrigin.position).normalized; // google said this was the direction between 2 points
            Debug.DrawLine(Cam.position, aimDirection, Color.red, Mathf.Infinity);
            ammo--;
            endTime = GetWaitEndTime(coolDownTimer);
            GameObject shootyThing = ObjectPooler.SpawnFromPool(Tag, ShootOrigin.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            EnableWandParticles();


            Rigidbody rb;
            if (shootyThing.TryGetComponent(out rb) == false)
                rb = shootyThing.AddComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
            rb.drag = 0;
            rb.angularDrag = 0;
            rb.angularVelocity = Vector3.zero;
            shootyThing.transform.position = ShootOrigin.position;
            rb.AddForce(shootyThing.transform.forward * shootForce);

            return shootyThing.GetComponent<Projectile>();
        }
        else
        {
            Debug.Log("No ammo left");
            return null;
        }
    }

    private void EnableWandParticles()
    {
        GameObject wandEffect = ObjectPooler.SpawnFromPool(WandEffectPoolTag, ShootOrigin.position, Quaternion.identity);
    }

}
