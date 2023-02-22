using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { None, UsableItem, KeyItem, CraftableItem }
    public new string name = "New Item";
    public Sprite icon = null;
    public ItemType itemType = ItemType.None;
}
