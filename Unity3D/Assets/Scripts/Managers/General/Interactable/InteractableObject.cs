using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(RigidBodyNoiseStimulus))]
public class InteractableObject : MonoBehaviour
{
    #region Required
    protected Rigidbody rb;
    protected RigidBodyNoiseStimulus rigidbodyNoise;
    #endregion Required

    [Header("General Interactable Settings")]
    [SerializeField] protected bool canPickUp = false;

    [Tooltip("Index of Player UI - Interactable Manager sprite")]
    [SerializeField] protected int selectIconIdx = -1;

    protected InteractableUIManager _interactableUIManager;

    #region Getters
    public bool CanPickUp() => canPickUp;

    #endregion Getters
    protected void Start()
    {
        rigidbodyNoise = GetComponent<RigidBodyNoiseStimulus>();
        rb = GetComponent<Rigidbody>();
        _interactableUIManager = UIManager.Instance.InteractableUIManager;
    }

    public void Select()   => _interactableUIManager.SelectIcon(selectIconIdx);
    public void Deselect() => _interactableUIManager.DeselectIcon();
}