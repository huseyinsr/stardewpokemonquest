using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Vector3 cameraRotation = Vector3.zero;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    public Vector3 GetTargetPosition()
    {
        return transform.position + positionOffset;
    }

    public Quaternion GetTargetRotation()
    {
        return Quaternion.Euler(cameraRotation);
    }
}
