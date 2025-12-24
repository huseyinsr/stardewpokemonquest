using UnityEngine;
using UnityEngine.UIElements;

public class DirtSlot : MonoBehaviour
{
    public bool IsOccupied => currentPlant != null;

    private Plant currentPlant;
    private GameObject ghostInstance;
    [SerializeField] private float position = 2f;

    public void ShowGhost(GameObject ghostPrefab)
    {
        if (IsOccupied || ghostInstance != null) return;

        ghostInstance = Instantiate(
            ghostPrefab,
            transform.position + Vector3.up * 0.01f,
            Quaternion.identity
        );
    }

    public void HideGhost()
    {
        if (ghostInstance != null)
            Destroy(ghostInstance);
    }

    public void PlantSeed(SeedItem seed)
    {
        if (IsOccupied) return;

        HideGhost();

        Vector3 spawnPosition = transform.position + Vector3.up * position;

        GameObject plantGO = Instantiate(
            seed.plantPrefab,
            spawnPosition,
            Quaternion.identity
        );

        currentPlant = plantGO.GetComponent<Plant>();
        currentPlant.Init(seed);
    }

}

