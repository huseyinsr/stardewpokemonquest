using UnityEngine;
using UnityEngine.EventSystems;

public class InvItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SeedItem seedData;
    public Transform originalParent; 

    private Canvas canvas;
    private DirtSlot hoveredSlot;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            DirtSlot slot = hit.collider.GetComponent<DirtSlot>();
            if (slot != hoveredSlot)
            {
                hoveredSlot?.HideGhost();
                hoveredSlot = slot;

                if (hoveredSlot != null)
                    hoveredSlot.ShowGhost(seedData.ghostPrefab);
            }
        }
        else
        {
            hoveredSlot?.HideGhost();
            hoveredSlot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        hoveredSlot?.HideGhost();

        if (hoveredSlot != null && !hoveredSlot.IsOccupied)
        {
            hoveredSlot.PlantSeed(seedData);
            Destroy(gameObject);
            return;
        }

        if (eventData.pointerEnter?.GetComponent<InvSlot>() == null)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
