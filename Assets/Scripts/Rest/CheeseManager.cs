using System.Collections;
using UnityEngine;

public class CheeseAndShitManager : MonoBehaviour
{
    [SerializeField] private GameObject cheeseObject;
    [SerializeField] private GameObject shitObject;
    [SerializeField] private float actionDelay = 0.5f;

    private CamereChange cameraChangeScript;
    private Quaternion initialCameraTargetRotation;
    private bool isWaitingForRotation = false;

    private void OnEnable()
    {
        CheeseSpawn.OnCheeseSpawned += HandleCheeseSpawned;
    }

    private void OnDisable()
    {
        CheeseSpawn.OnCheeseSpawned -= HandleCheeseSpawned;
    }

    private void Start()
    {
        if (cheeseObject != null) cheeseObject.SetActive(false);
        if (shitObject != null) shitObject.SetActive(false);

        cameraChangeScript = Object.FindFirstObjectByType<CamereChange>();
    }

    private void HandleCheeseSpawned()
    {
        if (cheeseObject != null) cheeseObject.SetActive(true);

        if (cameraChangeScript != null)
        {
            initialCameraTargetRotation = cameraChangeScript.GetCurrentTargetRotation();
            isWaitingForRotation = true;
        }
    }

    private void Update()
    {
        if (!isWaitingForRotation || cameraChangeScript == null) return;

        float currentYaw = cameraChangeScript.GetCurrentTargetRotation().eulerAngles.y;
        float initialYaw = initialCameraTargetRotation.eulerAngles.y;

        if (Mathf.Abs(Mathf.DeltaAngle(initialYaw, currentYaw)) > 1f)
        {
            isWaitingForRotation = false;
            StartCoroutine(SwitchItemsRoutine());
        }
    }

    private IEnumerator SwitchItemsRoutine()
    {
        yield return new WaitForSeconds(actionDelay);

        if (cheeseObject != null) cheeseObject.SetActive(false);
        if (shitObject != null) shitObject.SetActive(true);
    }
}