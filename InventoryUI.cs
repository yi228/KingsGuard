using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    private bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    void Start()
    {
        inven = Inventory.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onChangeItem += RedrawSlotUI;

        inventoryPanel.SetActive(activeInventory);
        equipmentPanel.SetActive(activeInventory);

        SlotNum();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
            equipmentPanel.SetActive(activeInventory);
        }
    }
    private void SlotNum()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].slotNum = i;
    }
    private void RedrawSlotUI()
    {
        for(int i= 0; i < slots.Length; i++)
            slots[i].RemoveSlot();

        for(int i=0; i<inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
