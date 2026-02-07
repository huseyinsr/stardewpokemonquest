using UnityEngine;
using System.Collections;

public class BookMovementtt : MonoBehaviour
{
    [SerializeField] private Vector3 moveDistance;
    [SerializeField] private float moveDuration = 1f;

    private void Start()
    {
        StartCoroutine(MoveRoutine());
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
