using UnityEngine;
using UnityEngine.UI;

public class SmoothArrowScroll : MonoBehaviour
{
    public RectTransform slotsHolder;
    public Button rightButton;
    public Button leftButton;

    public float rightScrollAmount = 50f;
    public float leftScrollAmount = 20f;
    public float scrollDuration = 1f;

    private float centerX;
    private float targetX;
    private float timer;
    private bool isMoving;
    private float fromX;
    private bool isAtRight = false;
    private bool isAtLeft = false;

    void Start()
    {
        centerX = slotsHolder.localPosition.x;
        targetX = centerX;

        if (rightButton != null)
            rightButton.onClick.AddListener(OnRightClick);

        if (leftButton != null)
            leftButton.onClick.AddListener(OnLeftClick);

        UpdateButtonStates();
    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            float progress = timer / scrollDuration;

            if (progress >= 1f)
            {
                progress = 1f;
                isMoving = false;
                UpdateButtonStates();
            }

            float currentX = Mathf.Lerp(fromX, targetX, progress);
            Vector2 pos = slotsHolder.localPosition;
            pos.x = currentX;
            slotsHolder.localPosition = pos;
        }
    }

    void OnRightClick()
    {
        float currentX = slotsHolder.localPosition.x;
        fromX = currentX;

        if (isAtLeft)
        {
            targetX = centerX;
            isAtLeft = false;
        }
        else if (isAtRight)
        {
            return;
        }
        else
        {
            targetX = centerX + rightScrollAmount;
            isAtRight = true;
        }

        timer = 0f;
        isMoving = true;
        UpdateButtonStates();
    }

    void OnLeftClick()
    {
        float currentX = slotsHolder.localPosition.x;
        fromX = currentX;

        if (isAtRight)
        {
            targetX = centerX;
            isAtRight = false;
        }
        else if (isAtLeft)
        {
            return;
        }
        else
        {
            targetX = centerX - leftScrollAmount;
            isAtLeft = true;
        }

        timer = 0f;
        isMoving = true;
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        if (rightButton != null)
        {
            rightButton.interactable = !isAtRight;
        }

        if (leftButton != null)
        {
            leftButton.interactable = !isAtLeft;
        }
    }

    public void ResetToCenter()
    {
        fromX = slotsHolder.localPosition.x;
        targetX = centerX;
        timer = 0f;
        isMoving = true;
        isAtRight = false;
        isAtLeft = false;
        UpdateButtonStates();
    }
}