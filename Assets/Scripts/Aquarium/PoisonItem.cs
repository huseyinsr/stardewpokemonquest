using System;
using UnityEngine;

public class PoisonItem : MonoBehaviour
{
    public static event Action AquariumPoisoned;

    private void Start()
    {
        AquariumPoisoned?.Invoke();
    }
}
