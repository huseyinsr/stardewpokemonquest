using UnityEngine;

public class ZoomableObject : MonoBehaviour
{
    public CameraPosition cameraPosition;

    private void OnMouseDown()
    {
        if (cameraPosition != null && ZoomManager.Instance != null)
        {
            ZoomManager.Instance.ZoomTo(cameraPosition);
        }
    }
}