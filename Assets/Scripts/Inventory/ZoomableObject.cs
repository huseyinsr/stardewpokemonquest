using UnityEngine;

public class ZoomableObject : MonoBehaviour
{
    [SerializeField] private CameraPosition cameraPosition;

    private void OnMouseDown()
    {
        if (UIBlocker.IsPointerOverUI)
            return;

        if (cameraPosition != null && ZoomManager.Instance != null)
        {
            ZoomManager.Instance.ZoomTo(cameraPosition);
        }
    }
}
