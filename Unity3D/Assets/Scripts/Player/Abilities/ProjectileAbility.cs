using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public abstract class ProjectileAbility : AbilityBase, IPoolUser
{
    #region Exposed In Editor

    [Header("Settings for: All Projectiles")]
    [SerializeField] protected float ammo = Mathf.Infinity;
    [SerializeField] protected float coolDownTimer = 0f;
    [SerializeField] protected float hitTimer = 0f;

    #endregion Exposed In Editor

    #region Hidden In Editor
    public Transform ShootOrigin { protected get; set; } = null;
    [HideInInspector] protected float endTime = 0f;
    [HideInInspector] protected bool shootProjectileFlag = false;
    protected Animator animator;

    public ObjectPooler ObjectPooler { get; set; }
    [field: SerializeField][field: Tooltip("Tag is the projectile tag")]public string Tag { get; set; } = "";
    public string WandEffectPoolTag = "SpawnSparks";

    private int _isCastingHash;
    #endregion Hidden In Editor

    protected void Start()
    {
        _isCastingHash = Animator.StringToHash("isCasting");
        animator = PlayerManager.Instance.GetComponentInChildren<Animator>(); 
        ObjectPooler = ObjectPooler.Instance;
    }
    public void SetShootAnimation(bool isShooting)
    {
        animator.SetBool(_isCastingHash, isShooting);
    }

    public void PickUpAmmo(int count = 1)
    {
        ammo += 2f * (float)count;
    }
    public abstract void Shoot();

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
    protected void EnableWandParticles()
    {
        ObjectPooler.SpawnFromPool(WandEffectPoolTag, ShootOrigin.position, Quaternion.identity);
    }
}
