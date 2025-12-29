using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public InventorySlot selectedSlot;
    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        items.Add(item);

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.Refresh();
        }
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        items.Remove(itemToRemove);

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.Refresh();
        }
    }

    public void OnSlotClicked(InventorySlot clickedSlot)
    {
        if (selectedSlot == null)
        {
            if (!clickedSlot.IsEmpty())
            {
                selectedSlot = clickedSlot;
                selectedSlot.SetSelected(true);
            }
            return;
        }

        if (selectedSlot == clickedSlot)
        {
            selectedSlot.SetSelected(false);
            selectedSlot = null;
            return;
        }

        if (clickedSlot.IsEmpty())
        {
            clickedSlot.SetItem(selectedSlot.item);
            selectedSlot.Clear();
            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }
        else
        {
            ItemData temp = clickedSlot.item;
            clickedSlot.SetItem(selectedSlot.item);
            selectedSlot.SetItem(temp);
            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }
    }

    public void UseSelectedItem()
    {
        if (selectedSlot != null && selectedSlot.item != null)
        {
            RemoveItem(selectedSlot.item);
            selectedSlot.Clear();
            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }
    }

    public ItemData GetSelectedItem()
    {
        if (selectedSlot != null)
        {
            return selectedSlot.item;
        }
        return null;
    }
}