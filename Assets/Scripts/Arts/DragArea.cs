using UnityEngine;

public class DragArea : MonoBehaviour
{
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPoint;

    private bool hasSpawned = false;

    private void OnEnable()
    {
        MovablePoint.onEveryPointInCorrectPosition.AddListener(SpawnTheObject);
    }

    private void OnDisable()
    {
        MovablePoint.onEveryPointInCorrectPosition.RemoveListener(SpawnTheObject);
    }

    internal Vector3 Clamp(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minPoint.position.x, maxPoint.position.x);
        position.y = Mathf.Clamp(position.y, minPoint.position.y, maxPoint.position.y);
        position.z = Mathf.Clamp(position.z, minPoint.position.z, maxPoint.position.z);
        return position;
    }

    private void SpawnTheObject()
    {
        if (hasSpawned) return;

        if (prefabToSpawn == null || spawnPoint == null)
        {
            Debug.LogWarning("Missing prefab or spawn point!");
            return;
        }

        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;

        Debug.Log("Spawned!");
    }
}