using UnityEngine;
using System;

public class CrystalDetector : MonoBehaviour
{
    [SerializeField] GameObject greenLights;
    [SerializeField] GameObject redLights;

    public event Action OnCrystalDetected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        greenLights.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            greenLights.SetActive(true);
            redLights.SetActive(false);

            OnCrystalDetected?.Invoke();
        }
    }
}