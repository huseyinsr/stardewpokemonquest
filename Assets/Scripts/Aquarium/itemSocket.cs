using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }
}