using UnityEngine;

public class StunPiece : MonoBehaviour
{
    [SerializeField] private int correctOrder;
    [SerializeField] private Transform itemSocket;

    public int CorrectOrder => correctOrder;
    public Transform ItemSocket => itemSocket;
}