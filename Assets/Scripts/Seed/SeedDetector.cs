using UnityEngine;
using System;
public class SeedDetector : MonoBehaviour
{
    public event Action OnSeedDetected;
    [SerializeField] GameObject Seed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SeedAct"))
        {
            //Debug.Log("Seed Detected");
            OnSeedDetected?.Invoke();
            Seed.SetActive(true);
        }
    }
}
