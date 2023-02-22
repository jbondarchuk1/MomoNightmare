using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InventoryUIManager : MonoBehaviour, IUIDialog
{
    private bool canOpen = true;
    
    private Inventory inventory;
    private List<ItemData> inventoryItems;
    private List<SlotUIManager> slots = new List<SlotUIManager>();
    
    [SerializeField] private GameObject inventoryDialog;

    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryItems = inventory.ToList();
        for (int i = 0; i < inventoryDialog.transform.childCount; i++)
            slots.Add(inventoryDialog.transform.GetChild(i).GetComponent<SlotUIManager>());
        Inventory.OnInventoryChanged += ReloadItems;
    }
    private void OnDisable()
    {
        Inventory.OnInventoryChanged -= ReloadItems;
    }
    public void ReloadItems()
    {
        if (inventory == null) inventory = Inventory.Instance;
        inventoryItems = inventory.ToList();
        for (int i = 0; i < 9; i++)
        {
            if (inventoryItems.Count > i) slots[i].SetSlot(inventoryItems[i]);
            else slots[i].SetSlot(null);
        }
    }
    public bool CanOpen() => this.canOpen;
    public bool IsOpen() => inventoryDialog.activeInHierarchy;
    public void Open()
    {
        Cursor.lockState = Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        inventoryDialog.SetActive(true);
    }
    public void Close() 
    {
        if (!IsOpen()) return;

        Cursor.lockState = CursorLockMode.Locked;
        inventoryDialog.SetActive(false); 
        Time.timeScale = 1f;
    }
    public void SetCanOpen(bool canOpen) => this.canOpen = canOpen;
    public void Toggle()
    {
        if (!canOpen || IsOpen()) Close();
        else Open();
    }
}
