using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public static ZoomManager Instance;

    public Camera mainCamera;
    public GameObject zoomBackButton;

    public bool IsZoomed { get; private set; }
    public bool IsZoomCompleted { get; private set; }
    public float ZoomStartTime { get; private set; }

    private Vector3 originalPos;
    private Quaternion originalRot;
    private CameraPosition targetCameraPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        originalPos = mainCamera.transform.position;
        originalRot = mainCamera.transform.rotation;

        if (zoomBackButton != null)
            zoomBackButton.SetActive(false);
    }

    private void Update()
    {
        if (targetCameraPosition == null || !IsZoomed) return;

        Vector3 targetPos = targetCameraPosition.GetTargetPosition();
        Quaternion targetRot = targetCameraPosition.GetTargetRotation();

        float moveSpeed = 10f;

        // Pozisyon
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetPos,
            Time.deltaTime * moveSpeed
        );

        // Rotasyon
        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            targetRot,
            Time.deltaTime * moveSpeed
        );

        if (Time.time - ZoomStartTime > 0.3f)
        {
            IsZoomCompleted = true;
        }
    }

    public void ZoomTo(CameraPosition cameraPos)
    {
        if (IsZoomed) return;
        mainCamera.transform.rotation = Quaternion.identity;

        targetCameraPosition = cameraPos;
        IsZoomed = true;
        IsZoomCompleted = false;
        ZoomStartTime = Time.time;

        if (zoomBackButton != null)
            zoomBackButton.SetActive(true);
    }


    public void ExitZoom()
    {
        targetCameraPosition = null;
        mainCamera.transform.position = originalPos;
        mainCamera.transform.rotation = originalRot;
        IsZoomed = false;
        IsZoomCompleted = false;

        if (zoomBackButton != null)
            zoomBackButton.SetActive(false);
    }

    public void OnExitZoomButton()
    {
        ExitZoom();
    }
}