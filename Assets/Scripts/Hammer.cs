using System.Collections;
using UnityEngine;

public class AxeInteraction : MonoBehaviour
{
    [SerializeField] private string[] woodFormNames = { "Wood Form 1", "Wood Form 2", "Wood Form 3", "Wood Form 4", "Wood Form 5" };
    [SerializeField] private float woodChangeDelay = 0.1f;

    [SerializeField] private Vector3 rotationOffset = new Vector3(30f, 0f, 0f);
    [SerializeField] private Vector3 positionOffset = new Vector3(-0.5f, 0f, 0f);
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private AnimationCurve hitCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private GameObject[] woodForms;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int currentWoodIndex = 0;
    private bool isAtTarget = false;
    private bool isAnimating = false;

    private GameObject axeVisual;
    private Collider axeCollider;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        Transform visualTransform = transform.Find("AxeVisual");
        if (visualTransform != null)
        {
            axeVisual = visualTransform.gameObject;
        }

        axeCollider = GetComponent<Collider>();

        woodForms = new GameObject[woodFormNames.Length];
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        for (int i = 0; i < woodFormNames.Length; i++)
        {
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == woodFormNames[i] && obj.scene.isLoaded)
                {
                    woodForms[i] = obj;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (ZoomManager.Instance != null)
        {
            bool shouldBeActive = ZoomManager.Instance.IsZoomed;

            if (axeVisual != null && axeVisual.activeSelf != shouldBeActive)
            {
                axeVisual.SetActive(shouldBeActive);
            }

            if (axeCollider != null && axeCollider.enabled != shouldBeActive)
            {
                axeCollider.enabled = shouldBeActive;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isAnimating) return;

        if (!isAtTarget)
        {
            StartCoroutine(AnimateAxe(initialPosition + positionOffset, initialRotation * Quaternion.Euler(rotationOffset), true));
            StartCoroutine(DelayedWoodChange());
        }
        else
        {
            StartCoroutine(AnimateAxe(initialPosition, initialRotation, false));
        }
    }

    private IEnumerator AnimateAxe(Vector3 targetPos, Quaternion targetRot, bool hitting)
    {
        isAnimating = true;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float percentage = elapsed / animationDuration;
            float curveValue = hitCurve.Evaluate(percentage);

            transform.localPosition = Vector3.Lerp(startPos, targetPos, curveValue);
            transform.localRotation = Quaternion.Lerp(startRot, targetRot, curveValue);
            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localRotation = targetRot;

        isAtTarget = hitting;
        isAnimating = false;
    }

    private IEnumerator DelayedWoodChange()
    {
        yield return new WaitForSeconds(woodChangeDelay);

        if (currentWoodIndex < woodForms.Length - 1)
        {
            if (woodForms[currentWoodIndex] != null)
                woodForms[currentWoodIndex].SetActive(false);

            currentWoodIndex++;

            if (woodForms[currentWoodIndex] != null)
                woodForms[currentWoodIndex].SetActive(true);

            if (currentWoodIndex == woodForms.Length - 1)
            {
                //Debug.Log("Wood is destroyed!");
                Destroy(gameObject);
            }
        }
    }
}