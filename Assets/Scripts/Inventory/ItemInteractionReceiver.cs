using UnityEngine;

public class ItemInteractionReceiver : MonoBehaviour
{
    public ItemPrefabMapping[] itemPrefabMappings;
    public bool requiresZoom = true;
    public float minZoomTime = 0.2f;

    private bool[] usedFlags;

    private void Awake()
    {
        usedFlags = new bool[itemPrefabMappings.Length];
    }

    private void OnMouseDown()
    {
        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime) return;
        }

        InventorySlot selectedSlot = Inventory.Instance.GetSelectedSlot();
        if (selectedSlot == null || selectedSlot.IsEmpty()) return;

        int index = System.Array.FindIndex(
            itemPrefabMappings,
            m => m.item == selectedSlot.item
        );

        if (index == -1) return;
        if (usedFlags[index]) return;

        if (itemPrefabMappings[index].prefab != null)
        {
            Instantiate(itemPrefabMappings[index].prefab, transform.position, transform.rotation);
        }

        usedFlags[index] = true;
        Inventory.Instance.RemoveItem(selectedSlot);
    }
}