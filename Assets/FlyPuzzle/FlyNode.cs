using System.Collections.Generic;
using UnityEngine;

public class FlyNode : MonoBehaviour
{
    [SerializeField] private bool requiresZoom = true;
    [SerializeField] private float minZoomTime = 0.2f;
    [SerializeField] private List<FlyNode> nextNodes = new();

    private FlyPuzzleManager manager;
    private Collider nodeCollider;

    public IReadOnlyList<FlyNode> NextNodes => nextNodes;

    private void Awake()
    {
        nodeCollider = GetComponent<Collider>();
    }

    public void Initialize(FlyPuzzleManager puzzleManager)
    {
        manager = puzzleManager;
    }

    public void SetInteractable(bool value)
    {
        if (nodeCollider != null)
            nodeCollider.enabled = value;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (nodeCollider != null)
            nodeCollider.enabled = false;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (UIBlocker.IsPointerOverUI)
            return;

        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed)
                return;

            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime)
                return;
        }

        if (manager == null)
            return;

        manager.SelectNode(this);
    }
}