using UnityEngine;

public class AppleEating : MonoBehaviour
{
    [SerializeField] GameObject apple;
    [SerializeField] GameObject eatedApple;
    [SerializeField] bool requiresZoom = true;
    [SerializeField] float minZoomTime = 0.2f;


    private void Start()
    {
        apple.SetActive(true);
        eatedApple.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (requiresZoom)
        {
            if (!ZoomManager.Instance.IsZoomed) return;
            if (Time.time - ZoomManager.Instance.ZoomStartTime < minZoomTime) return;
        }
        apple.SetActive(false);
        eatedApple.SetActive(true);
    }

}
