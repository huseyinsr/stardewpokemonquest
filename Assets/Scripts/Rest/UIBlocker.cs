using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour
{
    public static bool IsPointerOverUI { get; private set; }

    private void Update()
    {
        IsPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
