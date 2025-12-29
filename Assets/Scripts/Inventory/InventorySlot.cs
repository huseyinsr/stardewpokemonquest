using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public ItemData item;

    private Color normalColor = Color.white;
    private Color selectedColor = Color.yellow;

    public bool IsEmpty()
    {
        return item == null;
    }

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        if (icon == null) return;

        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void Clear()
    {
        item = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnSlotClicked(this);
        }
    }

    public void SetSelected(bool selected)
    {
        if (icon != null)
        {
            icon.color = selected ? selectedColor : normalColor;
        }
    }
}