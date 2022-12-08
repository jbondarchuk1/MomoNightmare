using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableInteractableObject : InteractableObject, IDestructable
{
    [Header("Required Breakeable Settings")]
    public bool canBreak = false;
    public bool canPop = false;
    [SerializeField] private GameObject standardGameObject;
    
    [Space]
    
    [Header("Optional Breakeable Settings")]
    [SerializeField] private GameObject brokenObjectPrefab;
    private Rigidbody[] brokenRigidBodies;

    protected new void Start()
    {
        standardGameObject.SetActive(true);
        
        if (brokenObjectPrefab != null)
        {
            AddRigidBodiesToChildren(brokenObjectPrefab);
            brokenRigidBodies = brokenObjectPrefab.GetComponentsInChildren<Rigidbody>();
            brokenObjectPrefab.SetActive(false);
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
        rb.AddForce(Vector3.up * force + Vector3.right * Random.Range(-1f, 1f) * force / 2 + Vector3.forward * Random.Range(-1f, 1f) * force / 2);
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
}
