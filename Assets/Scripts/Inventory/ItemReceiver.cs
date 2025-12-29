using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public string requiredItemID;

    private void OnMouseDown()
    {
        ItemData selectedItem = Inventory.Instance.GetSelectedItem();

        if (selectedItem == null) return;

        if (selectedItem.itemID == requiredItemID)
        {
            Inventory.Instance.UseSelectedItem();
        }
    }
}