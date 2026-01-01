using UnityEngine;

public class ZoomableObject : MonoBehaviour
{
    [SerializeField] private CameraPosition cameraPosition;

    private void OnMouseDown()
    {
        if (cameraPosition != null && ZoomManager.Instance != null)
        {
            ZoomManager.Instance.ZoomTo(cameraPosition);
        }
    }
}
