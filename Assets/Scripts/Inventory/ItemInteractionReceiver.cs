using UnityEngine;

public class ItemInteractionReceiver : MonoBehaviour
{
    [Header("Item Mapping")]
    [SerializeField] private ItemPrefabMapping[] itemPrefabMappings;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Zoom Requirement")]
    [SerializeField] private bool requiresZoom = true;
    [SerializeField] private float minZoomTime = 0.2f;

    private bool[] usedItemFlags;
    private bool[] usedSpawnFlags;

    private void Awake()
    {
        usedItemFlags = new bool[itemPrefabMappings.Length];
        usedSpawnFlags = new bool[spawnPoints.Length];
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

        int itemIndex = System.Array.FindIndex(
            itemPrefabMappings,
            m => m.item == selectedSlot.item
        );

        if (itemIndex == -1) return;
        if (usedItemFlags[itemIndex]) return;

        int spawnIndex = GetNextFreeSpawnPointIndex();
        if (spawnIndex == -1)
        {
            Debug.LogWarning("No free spawn point available");
            return;
        }

        Transform spawnPoint = spawnPoints[spawnIndex];

        if (itemPrefabMappings[itemIndex].prefab != null)
        {
            Instantiate(
                itemPrefabMappings[itemIndex].prefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }

        usedItemFlags[itemIndex] = true;
        usedSpawnFlags[spawnIndex] = true;

        Inventory.Instance.RemoveItem(selectedSlot);
    }

    private int GetNextFreeSpawnPointIndex()
    {
        for (int i = 0; i < usedSpawnFlags.Length; i++)
        {
            if (!usedSpawnFlags[i])
                return i;
        }

        return -1;
    }
}
