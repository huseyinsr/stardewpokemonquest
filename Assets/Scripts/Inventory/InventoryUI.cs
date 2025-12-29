using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public InventorySlot[] slots;

    private void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        if (slots == null || slots.Length == 0) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
            {
                slots[i].SetItem(Inventory.Instance.items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
}