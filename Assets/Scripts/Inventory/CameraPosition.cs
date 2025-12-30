using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [Header("Absolute Camera Rotation")]
    public Vector3 cameraRotation = Vector3.zero;

    [Header("Absolute Position Offset")]
    public Vector3 positionOffset = Vector3.zero;

    public Vector3 GetTargetPosition()
    {
        return transform.position + positionOffset;
    }

    public Quaternion GetTargetRotation()
    {
        return Quaternion.Euler(cameraRotation);
    }
}
