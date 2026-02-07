using UnityEngine;

public class BookDetection : MonoBehaviour
{
    public event System.Action BookDetected;
    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Book"))
        {
            BookDetected?.Invoke();
        }
    }
}
