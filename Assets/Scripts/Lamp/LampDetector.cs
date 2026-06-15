using UnityEngine;
using System;
public class LampDetector : MonoBehaviour
{
    public event Action OnLampActivated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LampActiveating"))
        {
            //Debug.Log("Lamp Activated");
            OnLampActivated?.Invoke();
        }
    }
}
