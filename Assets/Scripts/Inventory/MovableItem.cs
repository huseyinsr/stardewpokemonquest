using UnityEngine;

public class MovableItem : MonoBehaviour
{
    private void OnMouseDrag()
    {
        if (!ZoomManager.Instance.IsZoomed) return;

        Vector3 mouse = Input.mousePosition;
        mouse.z = 1.5f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
        transform.position = worldPos;
    }
}
