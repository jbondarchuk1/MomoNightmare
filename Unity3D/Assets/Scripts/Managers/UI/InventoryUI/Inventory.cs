using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    // name, item
    protected Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();

    public delegate void UpdateInventory();
    public static event UpdateInventory OnInventoryChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void Add(Item item)
    {
        string name = item.name;
        if (items.ContainsKey(name)) items[name].Count++;
        else items.Add(name, new ItemData(item));
        OnInventoryChanged?.Invoke();
    }
    public Item Remove(string itemName)
    {
        if (items.TryGetValue(itemName, out ItemData data))
        {
            Item item = null;
            if (data.Count > 0)
            {
                data.Count = Mathf.Max(0, data.Count - 1);
                item = data.ItemType;
            }
            OnInventoryChanged?.Invoke();
            return item;
        }
        Debug.LogWarning("Inventory does not contain: " + itemName);
        return null;
    }
    public List<ItemData> ToList()
    {
        return items.Values.ToList();
    }

    public void Use(string itemName)
    {
        Item item = Remove(itemName);
        if (item != null) item.Use();
    }
}
