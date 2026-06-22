using System;
using System.Collections;
using UnityEngine;

public class KeyUnlocker : MonoBehaviour
{
    public static event Action OnKeyPlaced;
    [SerializeField] private float destroyDelay = 1.0f;

    void Start()
    {
        OnKeyPlaced?.Invoke();
        Destroy(gameObject, destroyDelay);
    }
}