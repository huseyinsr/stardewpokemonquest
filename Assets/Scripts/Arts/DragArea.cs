using UnityEngine;

public class DragArea : MonoBehaviour
{
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;

    internal Vector3 Clamp(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minPoint.position.x, maxPoint.position.x);
        position.y = Mathf.Clamp(position.y, minPoint.position.y, maxPoint.position.y);
        position.z = Mathf.Clamp(position.z, minPoint.position.z, maxPoint.position.z);
        return position;
    }
}
