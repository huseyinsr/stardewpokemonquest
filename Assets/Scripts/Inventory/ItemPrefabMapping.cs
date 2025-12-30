using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemPrefabMapping")]
public class ItemPrefabMapping : ScriptableObject
{
    public ItemData item;
    public GameObject prefab;
}