using System.Collections;
using UnityEngine;

public class DrawerMovement : MonoBehaviour
{
    [SerializeField] private Transform movableDrawerPart;
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0, 0, -0.5f);
    [SerializeField] private float openDuration = 1.0f;
    [SerializeField] private bool requiresZoom = true;
    [SerializeField] private float minZoomTime = 0.2f;
    [SerializeField] private GameObject linkedItem;

    private Vector3 initialPosition;
    private bool isUnlocked = false;
    private bool isOpened = false;
    private bool isMoving = false;

    void Start()
    {
        if (movableDrawerPart != null) initialPosition = movableDrawerPart.localPosition;
    }

    void OnEnable() => KeyUnlocker.OnKeyPlaced += UnlockDrawer;
    void OnDisable() => KeyUnlocker.OnKeyPlaced -= UnlockDrawer;

    void UnlockDrawer()
    {
        isUnlocked = true;

        if (linkedItem != null)
        {
            SphereCollider sphereCollider = linkedItem.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                Destroy(sphereCollider);
            }
        }
    }

    void OnMouseDown()
    {
        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime) return;
        }

        if (!isUnlocked || isMoving) return;

        if (!isOpened)
        {
            StartCoroutine(MoveDrawerCoroutine(initialPosition + openPositionOffset, true));
        }
        else
        {
            StartCoroutine(MoveDrawerCoroutine(initialPosition, false));
        }
    }

    IEnumerator MoveDrawerCoroutine(Vector3 targetPosition, bool targetOpenState)
    {
        isMoving = true;
        float time = 0;
        Vector3 startPosition = movableDrawerPart.localPosition;

        while (time < openDuration)
        {
            time += Time.deltaTime;
            movableDrawerPart.localPosition = Vector3.Lerp(startPosition, targetPosition, time / openDuration);
            yield return null;
        }

        movableDrawerPart.localPosition = targetPosition;
        isOpened = targetOpenState;
        isMoving = false;
    }
}