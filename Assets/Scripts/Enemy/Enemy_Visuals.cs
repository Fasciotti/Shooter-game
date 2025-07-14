using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Visuals : MonoBehaviour
{

    [Header("WeaponType")]
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    private Enemy_WeaponModel[] weaponModels;
    private GameObject currentWeaponModel;

    [Header("Color")]   
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;
    [SerializeField] private Texture[] textures;


    void Start()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        InvokeRepeating(nameof(SetupLook), 1, 1);
    }

    public GameObject CurrentWeaponModel()
    {
        return currentWeaponModel;
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
    }

    public void SetEnemyWeaponType(Enemy_MeleeWeaponType type)
    {
        weaponType = type;
    }

    private void SetupRandomWeapon()
    {
        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (Enemy_WeaponModel model in weaponModels)
        {
            // Deactivates every model first
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponType)
            {
                filteredWeaponModels.Add(model);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);

    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, textures.Length);

        Material newMat = new Material(skinnedMesh.material);

        newMat.mainTexture = textures[randomIndex];

        skinnedMesh.material = newMat;
    }
}
