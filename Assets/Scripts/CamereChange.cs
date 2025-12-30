using System.Collections;
using UnityEngine;

public class CamereChange : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float mouseInfluence = 5f;
    [SerializeField] private float mouseSmoothing = 6f;
    [SerializeField] private float cooldownDuration = 1f;
    [SerializeField] private float verticalStepDegrees = 10f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private GameObject[] cameraControlButtons;


    private Quaternion targetRotation;
    private bool isRotating;
    private bool canRotate = true;
    private Coroutine rotationCooldownCoroutine;

    private enum VerticalPosition
    {
        Normal,
        Up,
        Down
    }

    private VerticalPosition verticalPosition = VerticalPosition.Normal;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        targetRotation = mainCamera.transform.rotation;
    }

    private void Update()
    {
        Transform t = mainCamera.transform;

        Vector2 mouse = Input.mousePosition;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;

        float nx = halfWidth > 0f ? Mathf.Clamp((mouse.x - halfWidth) / halfWidth, -1f, 1f) : 0f;
        float ny = halfHeight > 0f ? Mathf.Clamp((mouse.y - halfHeight) / halfHeight, -1f, 1f) : 0f;

        Vector3 mouseOffsetEuler = new Vector3(
            -ny * mouseInfluence,
            0f,
            -nx * (mouseInfluence * 0.25f)
        );

        Quaternion desiredRotation = targetRotation * Quaternion.Euler(mouseOffsetEuler);
        float combinedSmooth = Mathf.Max(0.0001f, smoothSpeed) * Time.deltaTime;

        t.rotation = Quaternion.Slerp(
            t.rotation,
            desiredRotation,
            Mathf.Clamp01(combinedSmooth * (mouseSmoothing / smoothSpeed + 1f) * 0.5f)
        );

        if (isRotating && Quaternion.Angle(t.rotation, targetRotation) < 0.2f)
        {
            t.rotation = targetRotation;
            isRotating = false;
        }

        if (ZoomManager.Instance != null)
        {
            bool zoomed = ZoomManager.Instance.IsZoomed;

            if (cameraControlButtons != null)
            {
                for (int i = 0; i < cameraControlButtons.Length; i++)
                {
                    if (cameraControlButtons[i] != null)
                        cameraControlButtons[i].SetActive(!zoomed);
                }
            }
        }

    }

    public void OnLeftButtonClicked()
    {
        ExitZoomIfNeeded();
        ApplyYawDelta(-90f);
    }

    public void OnRightButtonClicked()
    {
        ExitZoomIfNeeded();
        ApplyYawDelta(90f);
    }

    public void OnUpButtonClicked()
    {
        ExitZoomIfNeeded();

        if (verticalPosition == VerticalPosition.Normal)
        {
            if (TryApplyPitchDelta(-verticalStepDegrees))
                verticalPosition = VerticalPosition.Up;
        }
        else if (verticalPosition == VerticalPosition.Down)
        {
            if (TryApplyPitchDelta(-verticalStepDegrees))
                verticalPosition = VerticalPosition.Normal;
        }
    }

    public void OnDownButtonClicked()
    {
        ExitZoomIfNeeded();

        if (verticalPosition == VerticalPosition.Normal)
        {
            if (TryApplyPitchDelta(verticalStepDegrees))
                verticalPosition = VerticalPosition.Down;
        }
        else if (verticalPosition == VerticalPosition.Up)
        {
            if (TryApplyPitchDelta(verticalStepDegrees))
                verticalPosition = VerticalPosition.Normal;
        }
    }

    public void OnleftButtonlicked()
    {
        OnLeftButtonClicked();
    }

    public void OnrightButtonlicked()
    {
        OnRightButtonClicked();
    }

    public void OnupButtonlicked()
    {
        OnUpButtonClicked();
    }

    public void OndownButtonlicked()
    {
        OnDownButtonClicked();
    }

    private void ExitZoomIfNeeded()
    {
        if (ZoomManager.Instance != null && ZoomManager.Instance.IsZoomed)
        {
            ZoomManager.Instance.ExitZoom();
        }
    }

    private void ApplyYawDelta(float deltaDegrees)
    {
        if (!canRotate) return;

        Vector3 euler = targetRotation.eulerAngles;
        euler.y = NormalizeAngle(euler.y + deltaDegrees);
        targetRotation = Quaternion.Euler(euler);
        isRotating = true;

        RestartCooldown();
    }

    private bool TryApplyPitchDelta(float deltaDegrees)
    {
        if (!canRotate) return false;

        Vector3 euler = targetRotation.eulerAngles;
        float signedPitch = SignedAngleFrom360(euler.x);
        signedPitch = Mathf.Clamp(signedPitch + deltaDegrees, minPitch, maxPitch);
        euler.x = NormalizeAngle(signedPitch);
        targetRotation = Quaternion.Euler(euler);
        isRotating = true;

        RestartCooldown();
        return true;
    }

    private void RestartCooldown()
    {
        if (rotationCooldownCoroutine != null)
            StopCoroutine(rotationCooldownCoroutine);

        rotationCooldownCoroutine = StartCoroutine(RotationCooldownCoroutine());
    }

    private IEnumerator RotationCooldownCoroutine()
    {
        canRotate = false;
        yield return new WaitForSeconds(cooldownDuration);
        canRotate = true;
        rotationCooldownCoroutine = null;
    }

    private static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    private static float SignedAngleFrom360(float angle360)
    {
        float a = NormalizeAngle(angle360);
        if (a > 180f) a -= 360f;
        return a;
    }
}
