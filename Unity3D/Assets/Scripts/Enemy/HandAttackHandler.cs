using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;
using EZCameraShake;

public class HandAttackHandler : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private BoxCollider collider;
    [SerializeField] private List<TrailRenderer> renderers = new List<TrailRenderer>();
    [SerializeField] private float cameraShakeAmount = 0.2f;
    private EnemyAnimationEventHandler enemyAnimationEventHandler;

    private bool canDamage = false;

    private void Update()
    {
        StartCoroutine(HandleScale());
    }

    private void Start()
    {
        enemyAnimationEventHandler = GetComponentInParent<EnemyAnimationEventHandler>();
        collider = GetComponent<BoxCollider>();
        enemyAnimationEventHandler.OnSwing += EnableHitbox;
        enemyAnimationEventHandler.OnAttack += DisableHitbox;
    }

    private IEnumerator HandleScale()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        float toScale = canDamage ? 1f : 0f;
        
        transform.localScale = new Vector3(transform.localScale.x, toScale, transform.localScale.z);
        yield return wait;
    }

    public void DisableHitbox()
    {
        collider.enabled = false;
        canDamage = false;
        foreach (TrailRenderer r in renderers)
            r.emitting = false;
    }
    public void EnableHitbox()
    {
        collider.enabled = true;
        canDamage = true;
        foreach (TrailRenderer r in renderers)
            r.emitting = true;

    }

    private void DamagePlayer()
    {
        CameraShaker.Instance.Shake(CameraShakePresets.Bump);
        PlayerManager.Instance.Damage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GetLayer(Layers.Target))
            DamagePlayer();
    }

}
