using UnityEngine;
using System.Collections;

public class BookCaseMovement : MonoBehaviour
{
    [SerializeField] private BookDetection bookDetection;
    [SerializeField] private Vector3 moveDistance;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private GameObject cameraPlace;

    private void Start()
    {
        bookDetection.BookDetected += OnBookDetected;
    }

    private void OnDestroy()
    {
        bookDetection.BookDetected -= OnBookDetected;
    }

    private void OnBookDetected()
    {
        StartCoroutine(MoveRoutine());
        Destroy(cameraPlace);
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + moveDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }
}
