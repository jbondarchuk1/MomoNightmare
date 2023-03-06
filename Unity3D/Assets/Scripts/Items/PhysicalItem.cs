using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

[RequireComponent(typeof(Item))]
[RequireComponent(typeof(ItemInteractableObject))]
public class PhysicalItem : MonoBehaviour
{
    public enum ItemType { Static, Floating, Rotating }
    public ItemType Type = ItemType.Static;

    public float height = 1f;
    public float speed = 1f;
    public Vector3 rotationVector;

    private float groundY;
    void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 40, GetMask(Layers.Ground)))
            groundY = hit.transform.position.y;
    }

    void Update()
    {
        switch (Type)
        {
            case ItemType.Floating:
                FloatItem();
                break;
            case ItemType.Rotating:
                FloatItem();
                RotateItem();
                break;
            default: break;
        }
    }

    private void FloatItem()
    {
        Vector3 pos = transform.position;
        pos.y = groundY + height;
        transform.position = pos;
    }
    private void RotateItem()
    {
        transform.Rotate(rotationVector);
    }
}
