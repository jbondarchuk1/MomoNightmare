using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    protected bool open = false;
    [Space]
    [Header("Lock Settings")]
    [SerializeField] bool locked = false;
    [SerializeField] private string keyName = "Key";
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

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
    public virtual void Activate()
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

    public void Unlock()
    {
        bool wasLocked = locked;
        locked = Inventory.Instance.Use(keyName, true) ? false : locked;
        if (wasLocked) StartCoroutine(ToggleUnlockCam());
    }

    private IEnumerator ToggleUnlockCam()
    {
        cinemachineVirtualCamera.enabled = true;
        PlayerManager.Instance.playerMovementManager.canMove = false;
        PlayerManager.Instance.uiManager.CinematicUIManager.Activate();
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance.uiManager.CinematicUIManager.Deactivate();
        PlayerManager.Instance.playerMovementManager.canMove = true;
        cinemachineVirtualCamera.enabled = false;
    }
}
