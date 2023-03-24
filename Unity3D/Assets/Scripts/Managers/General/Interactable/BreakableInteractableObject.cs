using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableInteractableObject : InteractableObject, IDestructable, IPoolUser
{
    [Header("Required Breakeable Settings")]
    public bool canBreak = false;
    public bool canPop = false;
    [SerializeField] private GameObject standardGameObject;
    
    [Space]
    
    [Header("Optional Breakeable Settings")]
    [SerializeField] private GameObject brokenObjectPrefab;
    private Rigidbody[] brokenRigidBodies;
    [SerializeField] private float breakImpulse = 1f;

    [Space]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string soundName = "Break";

    public ObjectPooler ObjectPooler { get; set; }
    [field: SerializeField] public string Tag { get; set; }

    protected new void Start()
    {
        ObjectPooler = ObjectPooler.Instance;
        standardGameObject.SetActive(true);
        audioManager = GetComponent<AudioManager>();
        if (brokenObjectPrefab != null)
        {
            AddRigidBodiesToChildren(brokenObjectPrefab);
            brokenRigidBodies = brokenObjectPrefab.GetComponentsInChildren<Rigidbody>();
            brokenObjectPrefab.SetActive(false);
        }
        else
        {
            brokenRigidBodies = new Rigidbody[] { GetComponent<Rigidbody>() };
        }
        base.Start();
    }

    /// <summary>
    /// Breaks this object and applies a random upward force
    /// </summary>
    /// <param name="force"></param>
    public void Pop(int force)
    {
        if (!canPop) return;
        Break();
        Vector3 dirforce = Vector3.up * force + Vector3.right * Random.Range(-1f, 1f) * force / 2 + Vector3.forward * Random.Range(-1f, 1f) * force / 2;
        rb.AddForce(dirforce);
        foreach (Rigidbody rb in brokenRigidBodies) rb.AddForce(dirforce);
        audioManager.PlaySound(soundName);

    }

    /// <summary>
    /// Breaks in place with a sound but no force.
    /// </summary>
    public void Break()
    {
        if (canBreak)
        {
            rigidbodyNoise.Break();
            DestroyObj();
        }
    }

    /// <summary>
    /// Warning: Interface implementation is deprecated, please use:
    ///     Pop() or Break() methods instead
    /// </summary>
    public void DestroyObj()
    {
        GameObject obj = standardGameObject;
        if (brokenObjectPrefab != null)
        {
            obj.SetActive(false);
            obj = brokenObjectPrefab;
            obj.SetActive(true);
        }
        if (Tag != "") ObjectPooler.SpawnFromPool(Tag, transform.position, transform.rotation);
    }

    private void AddRigidBodiesToChildren(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            if (!(child.gameObject.TryGetComponent(out Rigidbody _)))
                child.gameObject.AddComponent<Rigidbody>();
        }
    }

    public void ExplodeObj(Vector3 origin, float force)
    {
        DestroyObj();
        foreach (Rigidbody rb in brokenRigidBodies) 
            rb.AddExplosionForce(force*3, origin, force*5, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse);
        if (collision.impulse.magnitude >= breakImpulse)
            Break();
    }
}
