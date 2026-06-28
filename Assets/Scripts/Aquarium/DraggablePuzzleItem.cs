using UnityEngine;

public class DraggablePuzzleItem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float snapDistance = 1f;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;

    private float fixedX;
    private bool dragging;

    private ItemSocket currentSocket;

    private void Start()
    {
        fixedX = transform.position.x;

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        dragging = true;

        if (currentSocket != null)
        {
            currentSocket.SetOccupied(false);
            currentSocket = null;
        }

        transform.SetParent(null);
    }

    private void OnMouseUp()
    {
        dragging = false;

        StunPiece[] stuns = Object.FindObjectsByType<StunPiece>(FindObjectsSortMode.None);

        ItemSocket bestSocket = null;
        float bestDistance = snapDistance;

        foreach (var stun in stuns)
        {
            if (stun.ItemSocket == null)
                continue;

            ItemSocket socket = stun.ItemSocket.GetComponent<ItemSocket>();

            if (socket == null)
                continue;

            if (socket.IsOccupied)
                continue;

            float distance = Vector3.Distance(transform.position, stun.ItemSocket.position);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestSocket = socket;
            }
        }

        if (bestSocket != null)
        {
            currentSocket = bestSocket;
            currentSocket.SetOccupied(true);

            transform.position = bestSocket.transform.position;
            transform.SetParent(bestSocket.transform);
        }
    }

    private void Update()
    {
        if (!dragging)
            return;

        Plane plane = new Plane(Vector3.right, new Vector3(fixedX, 0, 0));
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 point = ray.GetPoint(enter);

            point.x = fixedX;
            point.y = Mathf.Clamp(point.y, minY, maxY);
            point.z = Mathf.Clamp(point.z, minZ, maxZ);

            transform.position = point;
        }
    }
}