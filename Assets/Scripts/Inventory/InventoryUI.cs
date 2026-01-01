using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private InventorySlot[] slots;

    private void Awake() => Instance = this;

    public void RefreshSlot(InventorySlot slot)
    {
        if (slot == null) return;
        slot.SetItem(slot.item);
    }

    public void RefreshAll()
    {
        foreach (var slot in slots) slot.SetItem(slot.item);
    }
}
