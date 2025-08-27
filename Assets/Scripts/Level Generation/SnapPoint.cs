using UnityEngine;

public enum SnapPointType { Enter, Exit }
public class SnapPoint : MonoBehaviour
{
    public SnapPointType SnapPointType; // Determines if this is an entrance or exit point

    // Updates the GameObject name in the editor to reflect the snap point type
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + SnapPointType;
    }
}
