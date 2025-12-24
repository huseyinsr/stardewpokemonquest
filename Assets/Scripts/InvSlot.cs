using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InvItem droppedItem = eventData.pointerDrag?.GetComponent<InvItem>();
        if (droppedItem == null) return;

        if (transform.childCount > 0)
        {
            Transform currentItem = transform.GetChild(0);

            currentItem.SetParent(droppedItem.originalParent);
            currentItem.localPosition = Vector3.zero;

            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;

            droppedItem.originalParent = transform;
        }
        else
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;
            droppedItem.originalParent = transform;
        }
    }
}
