using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private GameObject CrystalPoint;
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private GameObject notDetected;
    [SerializeField] private GameObject Ways;

    [SerializeField] private float WaysNormalVerticalSpeed = 100f;
    [SerializeField] private float WaysFastVerticalSpeed = 250f;
    [SerializeField] private float stepDistance = 50f;
    [SerializeField] private float horizontalSmoothTime = 0.1f;
    [SerializeField] private float fastDuration = 1f;
    [SerializeField] private float minZoomTime = 0.2f;
    [SerializeField] private bool requiresZoom = true;

    [SerializeField] private int requiredTrueWallCount = 1;
    private int currentTrueWallCount = 0;

    [SerializeField] private float normalShakeMagnitude = 3f;
    [SerializeField] private float fastShakeMagnitude = 5f;
    [SerializeField] private float normalShakeSpeed = 2f;
    [SerializeField] private float fastShakeSpeed = 4f;

    private bool isCrtystalDetected = false;
    private bool GameIsActive = false;
    private Vector3 waysStartPosition;
    private Vector3 rocketStartPosition;

    private float currentVerticalSpeed;
    private float noiseTimer = 0f;
    private float fastTimer = 0f;
    private bool isFastActive = false;

    private float targetHorizontalPos;
    private float horizontalVelocity = 0f;

    void Start()
    {
        miniGamePanel.SetActive(false);

        if (Ways != null)
        {
            waysStartPosition = Ways.transform.position;
            targetHorizontalPos = waysStartPosition.x;
        }

        if (rocket != null)
        {
            rocketStartPosition = rocket.transform.position;
        }

        currentVerticalSpeed = WaysNormalVerticalSpeed;

        Rocket rocketScript = rocket.GetComponent<Rocket>();
        if (rocketScript != null)
        {
            rocketScript.OnMiniGameWallDetected += OnMiniGameWallDetected;
            rocketScript.OnTrueWallDetected += OnTrueWallDetected;
        }
    }

    void Update()
    {
        CrystalPoint = GameObject.Find("CrystalPoint");
        CrystalDetector crystalDetector = CrystalPoint.GetComponent<CrystalDetector>();

        if (crystalDetector != null)
        {
            crystalDetector.OnCrystalDetected += Crystal;
        }

        if (isCrtystalDetected == true)
        {
            notDetected.SetActive(false);
            if (!GameIsActive) return;

            if (isFastActive)
            {
                fastTimer -= Time.deltaTime;
                if (fastTimer <= 0f)
                {
                    isFastActive = false;
                    currentVerticalSpeed = WaysNormalVerticalSpeed;
                }
            }

            float currentX = Mathf.SmoothDamp(Ways.transform.position.x, targetHorizontalPos, ref horizontalVelocity, horizontalSmoothTime);
            float currentY = Ways.transform.position.y - (currentVerticalSpeed * Time.deltaTime);

            Ways.transform.position = new Vector3(currentX, currentY, Ways.transform.position.z);

            HandleRocketAnimation();
        }
        else if (isCrtystalDetected == false)
        {
            notDetected.SetActive(true);
        }
    }

    private void HandleRocketAnimation()
    {
        if (rocket == null) return;

        float speedFactor = isFastActive ? fastShakeSpeed : normalShakeSpeed;
        float magnitudeFactor = isFastActive ? fastShakeMagnitude : normalShakeMagnitude;

        noiseTimer += Time.deltaTime * speedFactor;

        float noiseX = (Mathf.PerlinNoise(noiseTimer, 0f) - 0.5f) * magnitudeFactor;
        float noiseY = (Mathf.PerlinNoise(0f, noiseTimer) - 0.5f) * magnitudeFactor;

        rocket.transform.position = rocketStartPosition + new Vector3(noiseX, noiseY, 0f);
    }

    public void OnLeftButtonClick()
    {
        if (!GameIsActive || !isCrtystalDetected || Ways == null) return;
        targetHorizontalPos += stepDistance;
    }

    public void OnRightButtonClick()
    {
        if (!GameIsActive || !isCrtystalDetected || Ways == null) return;
        targetHorizontalPos -= stepDistance;
    }

    public void OnFastButtonClick()
    {
        if (!GameIsActive || !isCrtystalDetected) return;
        currentVerticalSpeed = WaysFastVerticalSpeed;
        fastTimer = fastDuration;
        isFastActive = true;
    }

    void OnMouseDown()
    {
        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime) return;
        }

        miniGamePanel.SetActive(true);
        GameIsActive = true;
    }

    private void Crystal()
    {
        isCrtystalDetected = true;
    }

    private void OnMiniGameWallDetected()
    {
        //UnityEngine.Debug.Log("Wall Detected");
        GameIsActive = false;
        miniGamePanel.SetActive(false);
        ResetMiniGame();
    }

    private void OnTrueWallDetected()
    {
        currentTrueWallCount++;

        if (currentTrueWallCount >= requiredTrueWallCount)
        {
            UnityEngine.Debug.Log("gone through all true ways");
            stepDistance = 0f;
        }
    }

    public void OnMiniGameExitButtonClicked()
    {
        miniGamePanel.SetActive(false);
        ResetMiniGame();
    }

    private void ResetMiniGame()
    {
        if (Ways != null)
        {
            Ways.transform.position = waysStartPosition;
            targetHorizontalPos = waysStartPosition.x;
        }

        if (rocket != null)
        {
            rocket.transform.position = rocketStartPosition;
            rocket.transform.rotation = Quaternion.identity;
        }

        noiseTimer = 0f;
        fastTimer = 0f;
        horizontalVelocity = 0f;
        isFastActive = false;
        currentVerticalSpeed = WaysNormalVerticalSpeed;
        currentTrueWallCount = 0;
    }
}