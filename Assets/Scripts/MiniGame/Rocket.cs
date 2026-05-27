using UnityEngine;
using System;

public class Rocket : MonoBehaviour
{
    public event Action OnMiniGameWallDetected;
    public event Action OnTrueWallDetected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            OnMiniGameWallDetected?.Invoke();
        }
        else if (other.CompareTag("TrueWall"))
        {
            OnTrueWallDetected?.Invoke();
        }
    }
}