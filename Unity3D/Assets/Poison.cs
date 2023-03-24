using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour, IActivatable, IPooledObject
{
    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float lifeSpan = Mathf.Infinity;

    PoisonHitbox poisonHitbox;

    public bool active = false;
    private Vector3 initialScale;
    Collider collider;
    private ParticleSystem poisonParticleSystem;
    public void Activate() => active = true;
    public void Deactivate() => active = false;
    public bool isActivated() => active;
    private void Awake()
    {
        poisonHitbox = GetComponentInChildren<PoisonHitbox>();
        collider = GetComponentInChildren<Collider>();
        poisonParticleSystem = GetComponentInChildren<ParticleSystem>();
        initialScale = collider.transform.localScale;
        collider.enabled = active;
    }
    private void Update()
    {
        poisonHitbox.active = active;
        collider.enabled = active;
        Vector3 toScale = active ? initialScale : collider.transform.localScale;
        collider.transform.localScale = Vector3.Lerp(collider.transform.localScale, toScale, Time.deltaTime * scaleSpeed);
    }


    public void OnObjectSpawn()
    {
        transform.rotation = Quaternion.identity;
        collider.transform.localScale = Vector3.zero;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerManager.GetMask(LayerManager.Layers.Obstruction, LayerManager.Layers.Ground)))
        {
            transform.position = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
        }

        Debug.Log("Spawning poison");

        Activate();
        if (poisonParticleSystem != null)
        {
            poisonParticleSystem.Play();
            if (lifeSpan != Mathf.Infinity) StartCoroutine(StopParticles());
        }

    }
    private IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(lifeSpan);
        poisonParticleSystem.Stop();
        Deactivate();
    }
}

