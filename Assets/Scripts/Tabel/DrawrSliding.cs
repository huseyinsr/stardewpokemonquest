using UnityEngine;
using System.Collections;

public class DrawrSliding : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float openDistance = 1f;
    [SerializeField] private Vector3 openDirection = Vector3.forward;

    [SerializeField] private float clickRadius = 0.5f;
    [SerializeField] private Vector3 interactionNormal = Vector3.up;

    [SerializeField] private Transform linkedItem;

    [SerializeField] private float minZoomWaitTime = 1f; 

    private Vector3 closedPosition;
    private Vector3 openedPosition;

    private bool isOpen;
    private bool isMoving;

    private Plane interactionPlane;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;

        closedPosition = transform.position;
        openedPosition = closedPosition + openDirection.normalized * openDistance;

        interactionPlane = new Plane(interactionNormal, closedPosition);
    }

    private void Update()
    {
        if (UIBlocker.IsPointerOverUI)
            return;
        if (!Input.GetMouseButtonDown(0)) return;

        if (ZoomManager.Instance != null)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomWaitTime) return; 
        }

        if (isMoving) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!interactionPlane.Raycast(ray, out float enter))
            return;

        Vector3 hitPoint = ray.GetPoint(enter);

        if (Vector3.Distance(hitPoint, transform.position) > clickRadius)
            return;

        StartCoroutine(Slide(
            isOpen ? openedPosition : closedPosition,
            isOpen ? closedPosition : openedPosition
        ));

        isOpen = !isOpen;
    }

    private IEnumerator Slide(Vector3 startPos, Vector3 targetPos)
    {
        isMoving = true;
        float t = 0f;

        while (t < speed)
        {
            Vector3 prevPos = transform.position;

            transform.position = Vector3.Lerp(startPos, targetPos, t / speed);

            Vector3 delta = transform.position - prevPos;

            if (linkedItem != null)
                linkedItem.position += delta;

            t += Time.deltaTime;
            yield return null;
        }

        Vector3 finalDelta = targetPos - transform.position;
        transform.position = targetPos;

        if (linkedItem != null)
            linkedItem.position += finalDelta;

        isMoving = false;
    }
}
