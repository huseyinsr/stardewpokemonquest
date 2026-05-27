using UnityEngine;
using System.Collections;

public class VaultManager : MonoBehaviour
{
    [SerializeField] private VaultBox[] boxes;
    [SerializeField] private int[] correctNumbers;

    [SerializeField] private Transform door;

    [Header("Door Target")]
    [SerializeField] private Transform doorTarget;
    [SerializeField] private float doorOpenSpeed = 1f;

    private Vector3 doorClosedPosition;
    private Quaternion doorClosedRotation;

    private bool doorIsOpen;

    private void Start()
    {
        foreach (var box in boxes)
            box.SetManager(this);

        if (door != null)
        {
            doorClosedPosition = door.position;
            doorClosedRotation = door.rotation;
        }
    }

    public void CheckCombination()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i].CurrentNumber != correctNumbers[i])
                return;
        }

        if (!doorIsOpen)
        {
            StartCoroutine(MoveDoorToTarget());
            doorIsOpen = true;
        }
    }

    private IEnumerator MoveDoorToTarget()
    {
        if (doorTarget == null)
            yield break;

        Vector3 startPos = door.position;
        Quaternion startRot = door.rotation;

        Vector3 targetPos = doorTarget.position;
        Quaternion targetRot = doorTarget.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * doorOpenSpeed;
            door.position = Vector3.Lerp(startPos, targetPos, t);
            door.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        door.position = targetPos;
        door.rotation = targetRot;
    }
}
