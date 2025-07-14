using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Visuals : MonoBehaviour
{
    [SerializeField] private Texture[] textures;
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(SetupRandomColor), 3, 3);
    }

    public void SetupLook()
    {
        SetupRandomColor();
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, textures.Length);

        Material newMat = new Material(skinnedMesh.material);

        newMat.mainTexture = textures[randomIndex];

        skinnedMesh.material = newMat;
    }
}
