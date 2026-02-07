using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItemPrefabMapping")]
public class ItemPrefabMapping : ScriptableObject
{
    [SerializeField] public ItemData item;
    [SerializeField] public GameObject prefab;
}
