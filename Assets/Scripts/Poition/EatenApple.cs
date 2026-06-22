using System.Collections;
using UnityEngine;

public class EatenApple : MonoBehaviour
{
    [SerializeField] private float pivotOffsetFactor = 0.2f;

    private void Start()
    {
        if (PotionManager.Instance != null)
        {
            PotionManager.Instance.RegisterEatenApple();
        }
    }

    private void OnEnable()
    {
        PotionManager.OnItemsAreInPosition += StartFadeOut;
    }

    private void OnDisable()
    {
        PotionManager.OnItemsAreInPosition -= StartFadeOut;
    }

    private void StartFadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float elapsed = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 startPosition = transform.position;

        Renderer renderer = GetComponentInChildren<Renderer>();
        float height = renderer != null ? renderer.bounds.size.y : 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float scale = Mathf.Lerp(1f, 0f, t);

            transform.localScale = startScale * scale;

            float offset = (1f - scale) * height * pivotOffsetFactor;

            transform.position = startPosition - Vector3.up * offset;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}