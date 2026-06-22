using System;
using UnityEngine;

public class CheeseSpawn : MonoBehaviour
{
    public static event Action OnCheeseSpawned;

    private void Start()
    {
        OnCheeseSpawned?.Invoke();
    }
}