using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelPart : MonoBehaviour
{
    public void SnapAndAlignLevelPart(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEnterSnapPoint();

        AlignTo(entrancePoint, targetSnapPoint); // Always before snapping
        SnapTo(entrancePoint, targetSnapPoint);
    }

    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate and apply position offset to match snap points
        var offset = transform.position - ownSnapPoint.transform.position;

        var newPosition = targetSnapPoint.transform.position + offset;

        transform.position = newPosition;


    }

    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate rotation difference and apply necessary rotations
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        transform.rotation = targetSnapPoint.transform.rotation;
        transform.Rotate(0, 180, 0); // Flip to face correct direction
        transform.Rotate(0, -rotationOffset, 0); // Apply offset correction

    }

    // Convenience methods to get specific snap points
    public SnapPoint GetExitSnapPoint() => GetSnapPointOfType(SnapPointType.Exit);
    public SnapPoint GetEnterSnapPoint() => GetSnapPointOfType(SnapPointType.Enter);

    private SnapPoint GetSnapPointOfType(SnapPointType type)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new();

        // Filter snap points by type
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if(snapPoint.SnapPointType == type)
                filteredSnapPoints.Add(snapPoint);
        }

        // Return a random snap point of the requested type
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);

            return filteredSnapPoints[randomIndex];
        }

        return null;
    }
}
