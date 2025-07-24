using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cover : MonoBehaviour
{
    private Player player;

    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private float zOffset = 1;
    [SerializeField] private float xOffset = 1;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private List<CoverPoint> coverPoints = new();

    private void Start()
    {
        GenerateCoverPoint();
        player = FindFirstObjectByType<Player>();
    }

    private void GenerateCoverPoint()
    {
        Vector3[] coverPointPosition = {
            new Vector3(0, yOffset, zOffset), // Forward
            new Vector3(0, yOffset, -zOffset), // Backward
            new Vector3(xOffset, yOffset, 0), // Right
            new Vector3(-xOffset, yOffset, 0)}; // Left


        foreach (var position in coverPointPosition)
        {
            Vector3 worldPosition = transform.TransformPoint(position);
            CoverPoint coverPoint = Instantiate(coverPointPrefab, worldPosition, Quaternion.identity, transform).GetComponent<CoverPoint>();

            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoints = new(); 

        foreach (CoverPoint coverPoint in coverPoints)
        {
            if (CheckCoverPoint(coverPoint, enemy))
                validCoverPoints.Add(coverPoint);
        }

        return validCoverPoints;
    }

    private bool CheckCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        Debug.Log(0);


        if (coverPoint.isOccupied)
            return false;

        Debug.Log(1);

        if (!IsCoverTheFurthestOne(coverPoint))
            return false;

        Debug.Log(2);


        if (IsCoverBehindPlayer(coverPoint, enemy))
            return false;
        Debug.Log(3);

        if (IsCoverCloseToPlayer(coverPoint))
            return false;
        Debug.Log(4);


        if (IsCoverCloseToLastCover(coverPoint, enemy))
            return false;

        Debug.Log(1);


        return true;
    }

    private bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemy)
    {
        CoverPoint currentCover = enemy.GetComponent<Enemy_Range>().currentCover;

        return currentCover != null &&
            Vector3.Distance(currentCover.transform.position, coverPoint.transform.position) < 3;
    }

    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, player.transform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemy.position);

        return distanceToPlayer < distanceToEnemy;
    }

    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(player.transform.position, coverPoint.transform.position) < 2;
    }
    private bool IsCoverTheFurthestOne(CoverPoint coverPoint)
    {
        CoverPoint furthestPoint = null;
        float furthestDistance = 0;

        foreach (CoverPoint point in coverPoints)
        {
            float currentDistance = Vector3.Distance(player.transform.position, point.transform.position);

            if ((currentDistance > furthestDistance))
            {
                furthestDistance = currentDistance;
                furthestPoint = point;
            }
        }

        return furthestPoint == coverPoint;
    }
}
