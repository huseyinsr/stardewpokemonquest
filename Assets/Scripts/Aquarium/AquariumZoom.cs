using UnityEngine;
using System.Collections.Generic;

public class AquariumZoom : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToDisable = new();

    private bool previousZoomState;

    private void Update()
    {
        if (ZoomManager.Instance == null)
            return;

        bool currentZoomState = ZoomManager.Instance.IsZoomed;

        if (!previousZoomState && currentZoomState)
        {
            SetObjectsActive(false);
        }

        if (previousZoomState && !currentZoomState)
        {
            SetObjectsActive(true);
        }

        previousZoomState = currentZoomState;
    }

    private void SetObjectsActive(bool active)
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}