using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector] public new string name = "";
    public ItemSO itemSO;
    [field: SerializeField] public bool CanUse   { get; private set; } = false;
    [field: SerializeField] public bool CanCraft { get; private set; } = false;
    public int maxHold = 100;

    private void Start()
    {
        name = itemSO.name;
        switch (itemSO.itemType)
        {
            case ItemSO.ItemType.UsableItem:
                CanUse = true;
                CanCraft = false;
                break;
            case ItemSO.ItemType.CraftableItem:
                CanUse = false;
                CanCraft = true;
                break;
            default:
                CanUse = false;
                CanCraft = false;
                break;
        }
    }
    public virtual void Obtain()
    {
        Inventory.Instance.Add(this);
        this.gameObject.SetActive(false);
    }
    public virtual bool Use(bool keyUsage = false)
    {
        if (!(CanUse || keyUsage)) return false;
        Inventory.Instance.Remove(this.name);
        return true;
    }

}
