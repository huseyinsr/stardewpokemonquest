using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData item;

    private void OnMouseDown()
    {
        if (item != null && Inventory.Instance != null)
        {
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
