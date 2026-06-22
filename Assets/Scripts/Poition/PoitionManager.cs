using System;
using System.Collections;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public static PotionManager Instance { get; private set; }

    public static event Action<float> OnItemsAreInPosition;

    [SerializeField] private GameObject emptyPotion;
    [SerializeField] private GameObject fullPotion;

    [SerializeField] private MeshRenderer[] emptyPotionMeshRenderers;
    [SerializeField] private SpriteRenderer[] emptyPotionSpriteRenderers;

    [SerializeField] private MeshRenderer[] fullPotionMeshRenderers;
    [SerializeField] private SpriteRenderer[] fullPotionSpriteRenderers;

    [SerializeField] private float fadeDuration = 1f;

    private bool isSnakeReady;
    private bool isEatenAppleReady;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSnake()
    {
        isSnakeReady = true;
        CheckIngredients();
    }

    public void RegisterEatenApple()
    {
        isEatenAppleReady = true;
        CheckIngredients();
    }

    private void CheckIngredients()
    {
        if (!isSnakeReady || !isEatenAppleReady)
            return;

        OnItemsAreInPosition?.Invoke(fadeDuration);

        StartCoroutine(FadePotionRoutine());
    }

    private IEnumerator FadePotionRoutine()
    {
        if (fullPotion != null)
        {
            fullPotion.SetActive(true);
        }

        foreach (MeshRenderer renderer in fullPotionMeshRenderers)
        {
            if (renderer == null) continue;

            foreach (Material material in renderer.materials)
            {
                if (!material.HasProperty("_Color")) continue;

                Color color = material.color;
                color.a = 0f;
                material.color = color;
            }
        }

        foreach (SpriteRenderer renderer in fullPotionSpriteRenderers)
        {
            if (renderer == null) continue;

            Color color = renderer.color;
            color.a = 0f;
            renderer.color = color;
        }

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;

            foreach (MeshRenderer renderer in emptyPotionMeshRenderers)
            {
                if (renderer == null) continue;

                foreach (Material material in renderer.materials)
                {
                    if (!material.HasProperty("_Color")) continue;

                    Color color = material.color;
                    color.a = Mathf.Lerp(1f, 0f, t);
                    material.color = color;
                }
            }

            foreach (SpriteRenderer renderer in emptyPotionSpriteRenderers)
            {
                if (renderer == null) continue;

                Color color = renderer.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                renderer.color = color;
            }

            foreach (MeshRenderer renderer in fullPotionMeshRenderers)
            {
                if (renderer == null) continue;

                foreach (Material material in renderer.materials)
                {
                    if (!material.HasProperty("_Color")) continue;

                    Color color = material.color;
                    color.a = Mathf.Lerp(0f, 1f, t);
                    material.color = color;
                }
            }

            foreach (SpriteRenderer renderer in fullPotionSpriteRenderers)
            {
                if (renderer == null) continue;

                Color color = renderer.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                renderer.color = color;
            }

            yield return null;
        }

        if (emptyPotion != null)
        {
            emptyPotion.SetActive(false);
        }
    }
}