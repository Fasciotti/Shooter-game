using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cover : MonoBehaviour
{

    [SerializeField] private GameObject coverPointPrefab;
    [SerializeField] private float zOffset = 1;
    [SerializeField] private float xOffset = 1;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private List<CoverPoint> coverPoints = new();

    private void Start()
    {
        GenerateCoverPoint();
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

    public List<CoverPoint> GetCoverPoints() => coverPoints;
}
