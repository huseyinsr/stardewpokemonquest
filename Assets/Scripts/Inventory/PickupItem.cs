using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData item;

    private void OnMouseDown()
    {
        if (item != null && Inventory.Instance != null)
        {
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}