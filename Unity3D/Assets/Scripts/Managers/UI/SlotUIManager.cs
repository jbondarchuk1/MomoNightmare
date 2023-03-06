using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIManager : MonoBehaviour
{
    private ItemData item;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI text;

    public void SetSlot(ItemData newItem)
    {
        if (newItem == null)
        {
            this.item = null;
            itemIcon.sprite = null;
            text.text = null;
            return;
        }

        this.item = newItem;
        itemIcon.sprite = newItem.ItemType.itemSO.icon;
        text.text = newItem.Count.ToString();
    }

    public void OnClick()
    {
        Debug.Log("Clicked on item");
        if (item == null) return;
        Inventory.Instance.Use(item.ItemType.name);
    }
}
