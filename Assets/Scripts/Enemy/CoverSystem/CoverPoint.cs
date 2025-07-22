using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    public bool isOccupied = false;

    public void SetOccupied(bool occupied) => isOccupied = occupied;
}
