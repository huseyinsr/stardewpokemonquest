using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPuzzleManager : MonoBehaviour
{
    [SerializeField] private Transform specialItem;
    [SerializeField] private float specialItemAppearDuration = 1.5f;

    private FlyNode[] nodes;

    private readonly HashSet<FlyNode> removedNodes = new();

    private bool wasZoomed;
    private bool solved;

    private Vector3 specialItemScale;

    private void Start()
    {
        if (solved)
            return;

        foreach (var node in nodes)
        {
            node.Hide();
            node.SetInteractable(false);
        }
    }

    private void Awake()
    {
        nodes = GetComponentsInChildren<FlyNode>(true);

        foreach (var node in nodes)
        {
            node.Initialize(this);
        }

        if (specialItem != null)
        {
            specialItemScale = specialItem.localScale;
            specialItem.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        bool zoomed = ZoomManager.Instance != null &&
                      ZoomManager.Instance.IsZoomed;

        if (solved)
        {
            if (!zoomed)
            {
                foreach (var node in nodes)
                {
                    if (node.gameObject.activeSelf)
                        node.Hide();
                }
            }

            wasZoomed = zoomed;
            return;
        }

        if (zoomed && !wasZoomed)
        {
            StartPuzzle();
        }

        if (!zoomed && wasZoomed)
        {
            ResetPuzzle();
        }

        wasZoomed = zoomed;
    }

    private void StartPuzzle()
    {
        removedNodes.Clear();

        foreach (var node in nodes)
        {
            node.Show();
            node.SetInteractable(true);
        }
    }

    private void ResetPuzzle()
    {
        removedNodes.Clear();

        foreach (var node in nodes)
        {
            node.Show();
            node.SetInteractable(false);
            node.Hide();
        }
    }

    public void SelectNode(FlyNode node)
    {
        if (solved)
            return;

        if (!node.gameObject.activeSelf)
            return;

        removedNodes.Add(node);

        node.Hide();

        foreach (var n in nodes)
        {
            if (n.gameObject.activeSelf)
            {
                n.SetInteractable(false);
            }
        }

        foreach (var next in node.NextNodes)
        {
            if (next == null)
                continue;

            if (!removedNodes.Contains(next))
            {
                next.SetInteractable(true);
            }
        }

        if (removedNodes.Count == nodes.Length)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        solved = true;

        foreach (var node in nodes)
        {
            node.Hide();
        }

        if (specialItem != null)
        {
            StartCoroutine(ShowSpecialItem());
        }
    }

    private IEnumerator ShowSpecialItem()
    {
        specialItem.gameObject.SetActive(true);

        Vector3 startScale = specialItemScale * 0.2f;

        specialItem.localScale = startScale;

        float time = 0f;

        while (time < specialItemAppearDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / specialItemAppearDuration);

            specialItem.localScale =
                Vector3.Lerp(startScale, specialItemScale, t);

            yield return null;
        }

        specialItem.localScale = specialItemScale;
    }
}