using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place script on door mesh and collider. Parent needs to be the pivot.
/// </summary>
public class Door : InteractableObject, IActivatable, ILockable
{
    [Header("Door Settings")]
    [SerializeField] private float openRotation;
    [SerializeField] private float closeRotation;
    [SerializeField][Range(0,3)] private float speed = 1;
    private Transform parent;
    bool open = false;
    [Space]
    [Header("Lock Settings")]
    [SerializeField] bool locked = false;
    [SerializeField] private string keyName = "Key";

    private new void Start()
    {
        base.Start();
        parent = transform.parent;
    }
    private void Update()
    {
        float goalRot = open ? openRotation : closeRotation;
        Vector3 currRot = parent.localRotation.eulerAngles;
        Vector3 newRot = currRot;
        newRot.y = Mathf.Lerp(currRot.y, goalRot, Time.deltaTime* speed);
        parent.localRotation = Quaternion.Euler(newRot);
    }
    public void Activate()
    {
        if (locked) Unlock();
        open = !(locked || open) || open;
    }
    public void Deactivate() =>  open = false;
    public bool isActivated() => open;

    public void Lock()
    {
        if (!open && !locked) 
            locked = true;
    }

    public void Unlock() => locked = Inventory.Instance.Use(keyName, true) ? false: locked;
}
