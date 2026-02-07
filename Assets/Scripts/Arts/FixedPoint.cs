using UnityEngine;

public class FixedPoint : MonoBehaviour
{
    private MovablePoint occupiedBy;

    internal bool InternalIsOccupied()
    {
        return occupiedBy != null;
    }

    internal bool InternalTryOccupy(MovablePoint point)
    {
        if (occupiedBy != null) return false;

        occupiedBy = point;
        return true;
    }

    internal void InternalRelease()
    {
        occupiedBy = null;
    }

    internal Vector3 InternalGetPosition()
    {
        return transform.position;
    }
}
