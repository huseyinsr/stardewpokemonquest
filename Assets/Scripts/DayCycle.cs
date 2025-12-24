using UnityEngine;
using TMPro;
using System;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private int dayUI = 1;
    [SerializeField] private TextMeshProUGUI dayText;

    public event Action OnDayChanged;

    private void Start()
    {
        UpdateDayText();
    }

    public void NextDay()
    {
        dayUI += 1;
        UpdateDayText();;


        OnDayChanged?.Invoke();
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + dayUI;
        }
    }
}


