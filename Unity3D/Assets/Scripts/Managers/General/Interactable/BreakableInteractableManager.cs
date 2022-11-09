using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableInteractableManager : InteractableManager, IDestructable
{
    [Header("Required Breakeable Settings")]
    public bool canBreak = false;
    public bool canPop = false;
    [SerializeField][Tooltip("The Model Visible Before Breaking")] private GameObject standardGameObject;
    
    [Space]
    
    [Header("Optional Breakeable Settings")]
    [SerializeField][Tooltip("The Model Visible After Breaking")] private GameObject brokenObjectPrefab;

    protected new void Start()
    {
        base.Start();
        standardGameObject.SetActive(true);
        
        if (brokenObjectPrefab != null)
            brokenObjectPrefab.SetActive(false);
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
            // DestroyObj();
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

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!(child.gameObject.TryGetComponent(out Rigidbody _)))
                child.gameObject.AddComponent<Rigidbody>();
        }
    }
}
