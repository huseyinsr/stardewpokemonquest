using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public static ZoomManager Instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject zoomBackButton;
    [SerializeField] private float zoomSpeed = 5f;

    public bool IsZoomed { get; private set; }
    public bool IsZoomCompleted { get; private set; }
    public float ZoomStartTime { get; private set; }

    private Vector3 originalPos;
    private Quaternion originalCameraRotation;
    private Quaternion originalTargetRotation;

    private Vector3 zoomStartPos;
    private Quaternion zoomStartRot;

    private CameraPosition targetCameraPosition;
    private bool lockRotation;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalPos = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        if (zoomBackButton != null)
            zoomBackButton.SetActive(false);
    }

    private void Update()
    {
        if (UIBlocker.IsPointerOverUI)
            return;

        if (!IsZoomed || targetCameraPosition == null)
            return;

        float t = Mathf.Clamp01((Time.time - ZoomStartTime) * zoomSpeed);

        mainCamera.transform.position = Vector3.Lerp(
            zoomStartPos,
            targetCameraPosition.GetTargetPosition(),
            t
        );

        if (lockRotation)
        {
            mainCamera.transform.rotation = Quaternion.Slerp(
                zoomStartRot,
                targetCameraPosition.GetTargetRotation(),
                t
            );

            if (t >= 1f)
            {
                lockRotation = false;
                IsZoomCompleted = true;

                CamereChange camChange = mainCamera.GetComponent<CamereChange>();
                if (camChange != null)
                    camChange.SetBaseRotation(mainCamera.transform.rotation);
            }
        }
    }

    public void ZoomTo(CameraPosition cameraPos)
    {
        if (IsZoomed) return;

        zoomStartPos = mainCamera.transform.position;
        zoomStartRot = mainCamera.transform.rotation;

        targetCameraPosition = cameraPos;
        IsZoomed = true;
        IsZoomCompleted = false;
        ZoomStartTime = Time.time;
        lockRotation = true;

        originalPos = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        CamereChange camChange = mainCamera.GetComponent<CamereChange>();
        if (camChange != null)
            originalTargetRotation = camChange.GetCurrentTargetRotation();

        if (zoomBackButton != null)
            zoomBackButton.SetActive(true);
    }

    public void ExitZoom()
    {
        mainCamera.transform.position = originalPos;
        mainCamera.transform.rotation = originalCameraRotation;

        targetCameraPosition = null;
        IsZoomed = false;
        IsZoomCompleted = false;
        lockRotation = false;

        if (zoomBackButton != null)
            zoomBackButton.SetActive(false);

        CamereChange camChange = mainCamera.GetComponent<CamereChange>();
        if (camChange != null)
            camChange.ResetRotationToBase(originalTargetRotation);
    }

    public void OnExitZoomButton()
    {
        ExitZoom();
    }
}
