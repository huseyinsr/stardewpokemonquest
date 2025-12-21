using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 10f, 0f);
    [SerializeField, Range(0f, 1f)] private float _smoothTime = 0.12f;
    private Vector3 _velocity = Vector3.zero;
    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desired = _target.position + _offset;
        desired.y = _offset.y;

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, _smoothTime);
    }
}
