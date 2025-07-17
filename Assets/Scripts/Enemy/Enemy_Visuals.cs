using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Corruption Visuals")]
    [SerializeField] private int corruptionAmount;
    private GameObject[] corruptionCrystals;


    [Header("Weapon Visuals")]
    public GameObject currentWeaponModel { get; private set; }
    private Enemy_MeleeWeaponType weaponType;
    private Enemy_WeaponModel[] weaponModels;


    [Header("Skin Settings")]   
    [SerializeField] private Texture[] textures;
    private SkinnedMeshRenderer skinnedMesh;


    void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        
        CollectCrystalObjects();
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCrystals();
    }

    // Used to make sure the weapon the enemy is holding makes sense with its type
    public void SetEnemyWeaponType(Enemy_MeleeWeaponType type)
    {
        weaponType = type;
    }

    public void TrailEffectActive(bool active)
    {
        Enemy_WeaponModel currentWeaponScript = GetComponentInChildren<Enemy_WeaponModel>();
        currentWeaponScript.TrailEffectActive(active);
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

        AnimatorOverrideController animatorOverride =
            currentWeaponModel.GetComponent<Enemy_WeaponModel>().animatorOverride;

        if (weaponType == Enemy_MeleeWeaponType.Unarmed && animatorOverride != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = animatorOverride;
        }

    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, textures.Length);

        Material newMat = new Material(skinnedMesh.material);

        newMat.mainTexture = textures[randomIndex];

        skinnedMesh.material = newMat;
    }

    #region Crystals
    private void SetupRandomCrystals()
    {
        // Creates an availableIndexes of ints, that is going to represent what crystals are available to activate
        List<int> availableIndexes = new List<int>();

        // Assert it doesn't activate more crystals than exist
        corruptionAmount = Mathf.Min(corruptionAmount, corruptionCrystals.Length);

        for (int i = 0; i < corruptionCrystals.Length; i++)
        {
            availableIndexes.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        for (int i = 0; i < corruptionAmount; i++)
        {
            // Select a random index from the availableIndexes (crystals that have not yet been activate)
            int randomIndex = Random.Range(0, availableIndexes.Count);
            int objectIndex = availableIndexes[randomIndex];

            // Activate the object and remove its index from the list,
            // this way, we don't activate the same object again
            corruptionCrystals[objectIndex].SetActive(true);
            availableIndexes.RemoveAt(randomIndex);
        }
    }

    private void CollectCrystalObjects()
    {
        // Get all tag-scripts attached to the crystal objects, and use it to gather all gameObjects
        Enemy_CorruptionCrystals[] crystalsComponent = GetComponentsInChildren<Enemy_CorruptionCrystals>(true);
        corruptionCrystals = new GameObject[crystalsComponent.Length];

        for (int i = 0; i < crystalsComponent.Length; i++)
        {
            // Preparing the corruptionCrystal array, and deactivating all of them 
            corruptionCrystals[i] = crystalsComponent[i].gameObject;
        }
    }
    #endregion
}
