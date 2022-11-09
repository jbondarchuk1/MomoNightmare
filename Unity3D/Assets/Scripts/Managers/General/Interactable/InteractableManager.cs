using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(RigidBodyNoiseStimulus))]
public class InteractableManager : MonoBehaviour
{
    #region Required
    protected Rigidbody rb;
    protected RigidBodyNoiseStimulus rigidbodyNoise;
    #endregion Required

    [Header("General Interactable Settings")]
    [SerializeField] protected bool canPickUp = true;
    [SerializeField] protected bool canDamage = false;
    [SerializeField] protected bool isBodyPartGore = false;
    [SerializeField] protected int flatDamage = 0;

    protected void Start()
    {
        gameObject.TryGetComponent(out rb);
        gameObject.TryGetComponent(out rigidbodyNoise);
    }
}