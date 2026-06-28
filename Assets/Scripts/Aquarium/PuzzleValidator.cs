using UnityEngine;

public class PuzzleValidator : MonoBehaviour
{
    [SerializeField] private StunPuzzleManager stunPuzzleManager;

    public bool IsPuzzleSolved { get; private set; }

    private bool previousStunState;
    private bool previousItemState;
    private bool previousPuzzleState;

    private void Update()
    {
        bool stunOrderCorrect = stunPuzzleManager.AllStunsAreInCorrectPosition;
        bool itemOrderCorrect = CheckItems();

        IsPuzzleSolved = stunOrderCorrect && itemOrderCorrect;

        if (stunOrderCorrect && !previousStunState)
            Debug.Log("All stuns are in the correct positions");

        if (itemOrderCorrect && !previousItemState)
            Debug.Log("All items are on the correct stuns");

        if (IsPuzzleSolved && !previousPuzzleState)
            Debug.Log("Puzzle solved");

        previousStunState = stunOrderCorrect;
        previousItemState = itemOrderCorrect;
        previousPuzzleState = IsPuzzleSolved;
    }

    private bool CheckItems()
    {
        bool allCorrect = true;

        ItemForStun[] items = Object.FindObjectsByType<ItemForStun>(FindObjectsSortMode.None);

        foreach (ItemForStun item in items)
        {
            if (item.transform.parent == null)
            {
              //  Debug.Log(item.name + " is not on a stun");
                allCorrect = false;
                continue;
            }

            StunPiece stun = item.transform.parent.GetComponentInParent<StunPiece>();

            if (stun == null)
            {
                //Debug.Log(item.name + " is not on a stun");
                allCorrect = false;
                continue;
            }

            if (item.ItemID == stun.CorrectOrder)
            {
                //Debug.Log(item.name + " is on the correct stun");
            }
            else
            {
               // Debug.Log(item.name + " is on the wrong stun");
                allCorrect = false;
            }
        }

        return allCorrect;
    }
}