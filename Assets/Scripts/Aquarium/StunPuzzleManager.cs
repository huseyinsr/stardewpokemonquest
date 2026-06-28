using System.Linq;
using UnityEngine;

public class StunPuzzleManager : MonoBehaviour
{
    [SerializeField] private StunPiece[] stuns;

    public bool AllStunsAreInCorrectPosition { get; private set; }

    private StunPiece selectedStun;

    private void Update()
    {
        CheckOrder();
    }

    public void SelectStun(StunPiece stun)
    {
        if (selectedStun == null)
        {
            selectedStun = stun;
            return;
        }

        if (selectedStun == stun)
        {
            selectedStun = null;
            return;
        }

        SwapStuns(selectedStun, stun);
        selectedStun = null;
    }

    private void SwapStuns(StunPiece a, StunPiece b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;

        a.transform.position = new Vector3(
            posB.x,
            a.transform.position.y,
            posB.z
        );

        b.transform.position = new Vector3(
            posA.x,
            b.transform.position.y,
            posA.z
        );
    }

    private void CheckOrder()
    {
        var ordered = stuns
            .OrderBy(s => s.transform.position.z)
            .ToArray();

        AllStunsAreInCorrectPosition = true;

        for (int i = 0; i < ordered.Length; i++)
        {
            if (ordered[i].CorrectOrder != i)
            {
                AllStunsAreInCorrectPosition = false;
                break;
            }
        }
    }
}