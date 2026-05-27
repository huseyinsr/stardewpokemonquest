using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class VaultBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro numberText;
    [SerializeField] private int currentNumber = 0;

    private VaultManager manager;

    public int CurrentNumber => currentNumber;

    private void Start()
    {
        UpdateText();
    }

    public void SetManager(VaultManager m)
    {
        manager = m;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (ZoomManager.Instance != null && !ZoomManager.Instance.IsZoomed)
            return;

        currentNumber++;
        if (currentNumber > 9)
            currentNumber = 0;

        UpdateText();

        manager?.CheckCombination();
    }

    private void UpdateText()
    {
        if (numberText != null)
            numberText.text = currentNumber.ToString();
    }
}
