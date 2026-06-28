using UnityEngine;

public class StunClick : MonoBehaviour
{
    [SerializeField] private StunPuzzleManager puzzleManager;

    private StunPiece stunPiece;

    private void Awake()
    {
        stunPiece = GetComponent<StunPiece>();
    }

    private void OnMouseDown()
    {
        puzzleManager.SelectStun(stunPiece);
    }
}