using UnityEngine;
using UnityEngine.Events;

public class MovablePoint : MonoBehaviour
{
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private FixedPoint correctTarget;
    [SerializeField] private FixedPoint[] allFixedPoints;
    [SerializeField] private DragArea dragArea;

    public static UnityEvent onEveryPointInCorrectPosition = new UnityEvent();

    private Camera cam;
    private bool isDragging;
    private FixedPoint currentFixedPoint;

    private static MovablePoint[] allMovables;
    private static bool movementLocked;

    private void Start()
    {
        cam = Camera.main;
        allMovables = Object.FindObjectsByType<MovablePoint>(FindObjectsSortMode.None);
    }

    private void OnMouseDown()
    {
        if (ZoomManager.Instance != null && !ZoomManager.Instance.IsZoomed) return;

        if (movementLocked) return;

        isDragging = true;

        if (currentFixedPoint != null)
        {
            currentFixedPoint.InternalRelease();
            currentFixedPoint = null;
        }
    }

    private void OnMouseDrag()
    {
        if (ZoomManager.Instance != null && !ZoomManager.Instance.IsZoomed) return;

        if (movementLocked) return;
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        if (dragArea != null)
            worldPos = dragArea.Clamp(worldPos);

        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        if (ZoomManager.Instance != null && !ZoomManager.Instance.IsZoomed) return;

        isDragging = false;

        FixedPoint nearest = FindNearestAvailablePoint();

        if (nearest != null)
        {
            SnapTo(nearest);
        }

        CheckAllCorrect();
    }

    private FixedPoint FindNearestAvailablePoint()
    {
        FixedPoint nearest = null;
        float minDist = snapDistance;

        foreach (FixedPoint point in allFixedPoints)
        {
            if (point.InternalIsOccupied()) continue;

            float dist = Vector3.Distance(transform.position, point.InternalGetPosition());
            if (dist <= minDist)
            {
                minDist = dist;
                nearest = point;
            }
        }

        return nearest;
    }

    private void SnapTo(FixedPoint point)
    {
        if (!point.InternalTryOccupy(this)) return;

        transform.position = point.InternalGetPosition();
        currentFixedPoint = point;
    }

    private void CheckAllCorrect()
    {
        if (movementLocked) return;

        foreach (MovablePoint mp in allMovables)
        {
            if (mp.currentFixedPoint != mp.correctTarget)
                return;
        }

        movementLocked = true; 
        onEveryPointInCorrectPosition?.Invoke();

        Debug.Log("All points are in the correct position!");
    }

}
