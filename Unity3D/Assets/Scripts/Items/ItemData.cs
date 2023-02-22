using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public Item ItemType { get; set; }
    public int Count { get; set; }
    public ItemData(Item itemType)
    {
        this.ItemType = itemType;
        Count = 1;
    }
}
