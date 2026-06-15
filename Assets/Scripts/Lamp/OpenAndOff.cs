using UnityEngine;

public class OpenAndOff : MonoBehaviour
{
    [SerializeField] private LampDetector lampDetector;
    [SerializeField] private GameObject OpenedButton;
    [SerializeField] private GameObject ClosedButton;
    [SerializeField] private GameObject OpenedLight;
    [SerializeField] private GameObject ClosedLight;
    [SerializeField] private bool requiresZoom = true;
    [SerializeField] private float minZoomTime = 0.2f;

    private bool isLampPluggedIn = false;

    public static event System.Action<bool> OnLightStatusChanged;

    public bool IsLightOn => OpenedLight.activeSelf;

    private void Awake()
    {
        OpenedButton.SetActive(false);
        ClosedButton.SetActive(true);
        OpenedLight.SetActive(false);
        ClosedLight.SetActive(false);
    }

    private void OnEnable()
    {
        if (lampDetector != null)
        {
            lampDetector.OnLampActivated += HandleLampActivated;
        }
    }

    private void OnDisable()
    {
        if (lampDetector != null)
        {
            lampDetector.OnLampActivated -= HandleLampActivated;
        }
    }

    private void HandleLampActivated()
    {
        isLampPluggedIn = true;

        OpenedButton.SetActive(false);
        ClosedButton.SetActive(true);
        OpenedLight.SetActive(false);
        ClosedLight.SetActive(true);

        OnLightStatusChanged?.Invoke(false);
    }

    private void OnMouseDown()
    {
        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime) return;
        }

        if (OpenedButton.activeSelf)
        {
            OpenedButton.SetActive(false);
            ClosedButton.SetActive(true);

            if (isLampPluggedIn)
            {
                OpenedLight.SetActive(false);
                ClosedLight.SetActive(true);
            }
            OnLightStatusChanged?.Invoke(false);
        }
        else
        {
            OpenedButton.SetActive(true);
            ClosedButton.SetActive(false);

            if (isLampPluggedIn)
            {
                OpenedLight.SetActive(true);
                ClosedLight.SetActive(false);
            }
            OnLightStatusChanged?.Invoke(isLampPluggedIn);
        }
    }
}