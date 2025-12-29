using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CamereChange : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smoothSpeed = 8f;

    // New: how much the mouse position affects camera (degrees)
    [SerializeField] private float mouseInfluence = 5f;
    // New: additional smoothing factor for mouse-driven motion (kept similar to smoothSpeed)
    [SerializeField] private float mouseSmoothing = 6f;
    // New: cooldown duration in seconds to prevent button spam
    [SerializeField] private float cooldownDuration = 1f;

    // New: vertical (pitch) movement settings
    [SerializeField] private float verticalStepDegrees = 10f;
    [SerializeField] private float minPitch = -30f; // degrees (look down limit)
    [SerializeField] private float maxPitch = 60f;  // degrees (look up limit)

    private Quaternion targetRotation;
    private bool isRotating;

    // New runtime state for button cooldown
    private bool canRotate = true;
    private Coroutine rotationCooldownCoroutine;

    // New: track vertical one-shot state (Normal = can move up or down one time;
    // Up/Down = already moved once; pressing the opposite returns to Normal)
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

        // If this script is attached to the camera, use its transform; otherwise use mainCamera's transform.
        if (mainCamera != null)
        {
            targetRotation = mainCamera.transform.rotation;
        }
        else
        {
            targetRotation = transform.rotation;
        }
    }

    private void Update()
    {
        Transform t = (mainCamera != null) ? mainCamera.transform : transform;

        // Compute normalized mouse position relative to screen center in range approximately [-1,1]
        Vector2 mouse = Input.mousePosition;
        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;

        // Avoid division by zero on very small screens
        float nx = (halfWidth > 0f) ? Mathf.Clamp((mouse.x - halfWidth) / halfWidth, -1f, 1f) : 0f;
        float ny = (halfHeight > 0f) ? Mathf.Clamp((mouse.y - halfHeight) / halfHeight, -1f, 1f) : 0f;

        // Convert normalized values to subtle Euler offsets.
        // Up mouse (ny positive) should pitch camera down slightly (so invert ny for pitch sign).
        // Left/right can slightly roll or yaw; we apply a small roll to give a parallax feel.
        Vector3 mouseOffsetEuler = new Vector3(
            -ny * mouseInfluence,     // pitch
            0f,                       // keep yaw controlled by targetRotation (buttons)
            -nx * (mouseInfluence * 0.25f) // small roll for side tilt (reduced)
        );

        // Desired rotation includes the queued target rotation (from button clicks) plus the mouse offset.
        Quaternion desiredRotation = targetRotation * Quaternion.Euler(mouseOffsetEuler);

        // Smoothly rotate towards desired rotation. Use smoothSpeed to control overall responsiveness.
        float combinedSmooth = Mathf.Max(0.0001f, smoothSpeed) * Time.deltaTime;
        // Blend mouse smoothing a bit so mouse changes feel snappier if desired (lerp factor)
        t.rotation = Quaternion.Slerp(t.rotation, desiredRotation, Mathf.Clamp01(combinedSmooth * (mouseSmoothing / smoothSpeed + 1f) * 0.5f));

        // If we are performing a queued rotation (from button), check closeness to the pure targetRotation.
        // Use a slightly larger threshold to be robust to the mouse offset.
        if (isRotating && Quaternion.Angle(t.rotation, targetRotation) < 0.2f)
        {
            t.rotation = targetRotation; // snap to exact target (mouse offset ignored for the snap)
            isRotating = false;
        }
    }

    // Public methods to attach to UI buttons

    public void OnLeftButtonClicked()
    {
        ApplyYawDelta(-90f);
    }

    public void OnRightButtonClicked()
    {
        ApplyYawDelta(90f);
    }

    // New: vertical movement UI handlers
    public void OnUpButtonClicked()
    {
        // One-shot behavior:
        // - If currently Normal => move Up (one-time) and set state to Up.
        // - If currently Up => ignore (prevent repeated Up).
        // - If currently Down => move back to Normal (one step) and set state to Normal.
        if (verticalPosition == VerticalPosition.Normal)
        {
            if (TryApplyPitchDelta(-verticalStepDegrees))
            {
                verticalPosition = VerticalPosition.Up;
            }
        }
        else if (verticalPosition == VerticalPosition.Down)
        {
            if (TryApplyPitchDelta(-verticalStepDegrees))
            {
                verticalPosition = VerticalPosition.Normal;
            }
        }
        // else if already Up => ignore
    }

    public void OnDownButtonClicked()
    {
        // Symmetric to OnUpButtonClicked
        if (verticalPosition == VerticalPosition.Normal)
        {
            if (TryApplyPitchDelta(verticalStepDegrees))
            {
                verticalPosition = VerticalPosition.Down;
            }
        }
        else if (verticalPosition == VerticalPosition.Up)
        {
            if (TryApplyPitchDelta(verticalStepDegrees))
            {
                verticalPosition = VerticalPosition.Normal;
            }
        }
        // else if already Down => ignore
    }

    // Backwards-compatible methods matching original (misspelled) names in the file
    // so existing UI wiring won't break.
    public void OnleftButtonlicked()
    {
        OnLeftButtonClicked();
    }

    public void OnrightButtonlicked()
    {
        OnRightButtonClicked();
    }

    // Backwards-compatible misspelled vertical names (if any UI referenced them)
    public void OnupButtonlicked()
    {
        OnUpButtonClicked();
    }

    public void OndownButtonlicked()
    {
        OnDownButtonClicked();
    }

    // Helper: change the target rotation by yaw delta degrees
    private void ApplyYawDelta(float deltaDegrees)
    {
        // Prevent spamming: ignore input if still in cooldown
        if (!canRotate)
        {
            return;
        }

        // Use targetRotation so repeated clicks queue correctly relative to the pending target
        Vector3 euler = targetRotation.eulerAngles;
        euler.y = NormalizeAngle(euler.y + deltaDegrees);
        // Keep existing pitch/roll as-is
        targetRotation = Quaternion.Euler(euler);
        isRotating = true;

        // Start cooldown to prevent multiple rapid clicks
        if (rotationCooldownCoroutine != null)
        {
            StopCoroutine(rotationCooldownCoroutine);
        }
        rotationCooldownCoroutine = StartCoroutine(RotationCooldownCoroutine());
    }

    // Helper: original signature kept for compatibility, delegates to TryApplyPitchDelta
    private void ApplyPitchDelta(float deltaDegrees)
    {
        TryApplyPitchDelta(deltaDegrees);
    }

    // New: try to apply pitch change, returns true if rotation was started (respecting cooldown)
    private bool TryApplyPitchDelta(float deltaDegrees)
    {
        if (!canRotate)
        {
            return false;
        }

        Vector3 euler = targetRotation.eulerAngles;
        // Convert euler.x from [0,360) to signed [-180,180] for clamping
        float signedPitch = SignedAngleFrom360(euler.x);
        signedPitch = Mathf.Clamp(signedPitch + deltaDegrees, minPitch, maxPitch);
        // Convert back to 0..360 Euler representation for Quaternion.Euler
        float clampedEulerX = NormalizeAngle(signedPitch);
        euler.x = clampedEulerX;
        targetRotation = Quaternion.Euler(euler);
        isRotating = true;

        if (rotationCooldownCoroutine != null)
        {
            StopCoroutine(rotationCooldownCoroutine);
        }
        rotationCooldownCoroutine = StartCoroutine(RotationCooldownCoroutine());
        return true;
    }

    // Cooldown coroutine to prevent button spam
    private IEnumerator RotationCooldownCoroutine()
    {
        canRotate = false;
        yield return new WaitForSeconds(Mathf.Max(0f, cooldownDuration));
        canRotate = true;
        rotationCooldownCoroutine = null;
    }

    // Normalize angle to [0,360)
    private static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    // Convert 0..360 euler angle to signed -180..180
    private static float SignedAngleFrom360(float angle360)
    {
        float a = NormalizeAngle(angle360);
        if (a > 180f) a -= 360f;
        return a;
    } 
}
