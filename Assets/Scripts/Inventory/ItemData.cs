using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string itemID;
    [SerializeField] public string itemName;
    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
}
