using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemPrefabMapping")]
public class ItemPrefabMapping : ScriptableObject
{
    [SerializeField] public ItemData item;
    [SerializeField] public GameObject prefab;
}
