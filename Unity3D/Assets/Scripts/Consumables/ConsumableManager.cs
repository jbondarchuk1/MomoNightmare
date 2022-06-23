using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ConsumableManager : MonoBehaviour
{
    public enum ConsumablePhysics { floating, physical }

    public abstract GameObject ContactObject { get; set; }
    public abstract void Consume(GameObject contactObject);
    [SerializeField]private ConsumablePhysics type = ConsumablePhysics.floating;
    private Rigidbody rb = null;

    private void Start()
    {
        switch (type)
        {
            case ConsumablePhysics.floating:
                break;

            case ConsumablePhysics.physical:
                rb = GetComponent<Rigidbody>();
                rb = rb == null ? gameObject.AddComponent<Rigidbody>() : rb;

                break;
        }
    }
    private void Update()
    {
        HandleTypeRotation();
    }

    private void HandleTypeRotation()
    {
        switch (type)
        {
            case ConsumablePhysics.floating:
                transform.Rotate((Vector3.right + Vector3.back + Vector3.down) * 1f);
                break;

            case ConsumablePhysics.physical:
                break;
        }
    }
}

