using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] treeStages;
    [SerializeField] private SeedDetector seedDetector;
    [SerializeField] private OpenAndOff openAndOff;

    private int currentStageIndex = -1;
    private bool isSeedPlanted = false;
    private bool isLightOn = false;

    private void OnEnable()
    {
        if (seedDetector != null)
        {
            seedDetector.OnSeedDetected += HandleSeedDetected;
        }

        OpenAndOff.OnLightStatusChanged += HandleLightStatusChanged;
        WaterCan.OnSeedWatered += HandleSeedWatered;
    }

    private void OnDisable()
    {
        if (seedDetector != null)
        {
            seedDetector.OnSeedDetected -= HandleSeedDetected;
        }

        OpenAndOff.OnLightStatusChanged -= HandleLightStatusChanged;
        WaterCan.OnSeedWatered -= HandleSeedWatered;
    }

    private void Start()
    {
        foreach (GameObject stage in treeStages)
        {
            if (stage != null)
            {
                stage.SetActive(false);
            }
        }

        if (openAndOff != null)
        {
            isLightOn = openAndOff.IsLightOn;
        }
    }

    private void HandleSeedDetected()
    {
        isSeedPlanted = true;
    }

    private void HandleLightStatusChanged(bool status)
    {
        isLightOn = status;
    }

    private void HandleSeedWatered()
    {
        if (!isSeedPlanted || !isLightOn) return;

        if (currentStageIndex + 1 < treeStages.Length)
        {
            if (currentStageIndex >= 0 && treeStages[currentStageIndex] != null)
            {
                treeStages[currentStageIndex].SetActive(false);
            }

            currentStageIndex++;

            if (treeStages[currentStageIndex] != null)
            {
                treeStages[currentStageIndex].SetActive(true);
            }
        }
    }
}