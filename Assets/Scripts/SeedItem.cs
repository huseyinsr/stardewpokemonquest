using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Seed")]
public class SeedItem : ScriptableObject
{
    public string seedName;
    public GameObject plantPrefab;
    public GameObject ghostPrefab;
    public int maxGrowthDays;
}
