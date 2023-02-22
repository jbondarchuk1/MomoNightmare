using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemInteractableObject : InteractableObject, IActivatable
{
    private Item item;
    private new void Start()
    {
        base.Start();
        item = GetComponent<Item>();
    }
    public void Activate()
    {
        if (item == null)
        {
            Debug.LogError("Item is null for item interactable object");
            return;
        }
        item.Obtain();
    }

    public void Deactivate() { }
    public bool isActivated() => false;
}