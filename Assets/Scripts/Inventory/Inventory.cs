using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake() => Instance = this;

    public void AddItem(ItemData item)
    {
        if (item == null) return;
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                break;
            }
        }
        InventoryUI.Instance.RefreshAll();
    }

    public void RemoveItem(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty()) return;
        slot.Clear();
        InventoryUI.Instance.RefreshAll();
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        if (clickedSlot == null || clickedSlot.IsEmpty()) return;
        clickedSlot.SetSelected(!clickedSlot.isSelected);
        foreach (var slot in slots)
        {
            if (slot != clickedSlot) slot.SetSelected(false);
        }
    }

    public InventorySlot GetSelectedSlot() 
    {
        foreach (var slot in slots)
            if (slot.isSelected) return slot;
        return null;
    }

    public void UseSelectedItem()
    {
        InventorySlot slot = GetSelectedSlot();
        if (slot != null) RemoveItem(slot);
    }
}
