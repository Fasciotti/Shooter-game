using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public enum Enemy_MeleeWeaponType { Throw, OneHand, Unarmed }
public enum Enemy_RangeWeaponType { Pistol, Revolver, Shotgun, Rifle, AutoRifle }
public class Enemy_Visuals : MonoBehaviour
{
    [Header("Corruption Visuals")]
    public int corruptionAmount;

    [Header("Weapon Visuals")]
    public GameObject currentWeaponModel { get; private set; }

    [Header("Skin Settings")]
    [SerializeField] private Texture[] textures;
    private SkinnedMeshRenderer skinnedMesh;

    [Header("Enemy Range Properties")]
    [Space]
    [Header("Rig References")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private MultiAimConstraint weaponAimIK;
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandIKHint;

    [Header("Rig Settings")]
    private float leftHandIKTargetWeight;
    private float weaponAimIKTargetWeight;
    private float rigChangeRate;

    void Awake()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    private void Update()
    {

        if (TryGetComponent<Enemy_Range>(out _))
        {
            leftHandIK.weight = UpdateIKWeights(leftHandIK.weight, leftHandIKTargetWeight);
            weaponAimIK.weight = UpdateIKWeights(weaponAimIK.weight, weaponAimIKTargetWeight);
        }
    }

    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCrystals();

    }
    public void TrailEffectActive(bool active)
    {
        Enemy_WeaponModel currentWeaponScript = GetComponentInChildren<Enemy_WeaponModel>();

        currentWeaponScript.TrailEffectActive(active);
    }
    private void SetupRandomWeapon()
    {
        bool isEnemyMelee = GetComponent<Enemy_Melee>() != null;
        bool isEnemyRange = GetComponent<Enemy_Range>() != null;

        if (isEnemyMelee)
            currentWeaponModel = FindMeleeWeapon();

        if (isEnemyRange)
            currentWeaponModel = FindRangeWeapon();

        currentWeaponModel.SetActive(true);

        OverrideAnimatorIfPossible();
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
        GameObject[] corruptionCrystals = CollectCrystalObjects();

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
    private GameObject[] CollectCrystalObjects()
    {
        // Get all tag-scripts attached to the crystal objects, and use it to gather all gameObjects
        Enemy_CorruptionCrystals[] crystalsComponent = GetComponentsInChildren<Enemy_CorruptionCrystals>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalsComponent.Length];

        for (int i = 0; i < crystalsComponent.Length; i++)
        {
            // Preparing the corruptionCrystal array, and deactivating all of them 
            corruptionCrystals[i] = crystalsComponent[i].gameObject;
        }

        return corruptionCrystals;
    }
    #endregion
    private GameObject FindRangeWeapon()
    {
        Enemy_RangeWeaponModel[] weaponModels = GetComponentsInChildren<Enemy_RangeWeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;

        foreach (var model in weaponModels)
        {
            // First deactivate all models, as only one can be active at a time (Not needed, all models starts deactivated)
            model.gameObject.SetActive(false);

            if (weaponType == model.weaponType)
            {
                SwitchLayerAnimation(((int)model.weaponHoldType));
                SetupRigIKConstrains(model.leftHandIKTarget, model.leftHandIKHint);
                currentWeaponModel = model.gameObject;
            }
        }

        return currentWeaponModel;
    }


    private GameObject FindMeleeWeapon()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType;
        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var model in weaponModels)
        {
            // Deactivates every model first
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponType)
            {
                filteredWeaponModels.Add(model);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);
        return filteredWeaponModels[randomIndex].gameObject;
    }
    public void WeaponModelActive(bool active)
    {
        currentWeaponModel?.SetActive(active);
    }

    public void SecondaryWeaponModelActive(bool active)
    {
        FindSecondaryWeaponModel()?.SetActive(active);
    }

    public GameObject FindSecondaryWeaponModel()
    {
        Enemy_RangeSecondaryWeaponModel[] models =
            GetComponentsInChildren<Enemy_RangeSecondaryWeaponModel>(true);

        Enemy_Range enemy = GetComponent<Enemy_Range>();

        foreach (var model in models)
        {
            if (enemy.weaponType == model.weaponType)
            {
                return model.gameObject;
            }
        }

        return null;
    }

    public void GranadeModelActive(bool active)
    {
        TryGetComponent<Enemy_Range>(out var enemy);
        enemy?.GranadeStartPoint().gameObject.SetActive(active);
    }
    private void SwitchLayerAnimation(int index)
    {
        Animator animator = GetComponentInChildren<Animator>();

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);

        }

        animator.SetLayerWeight(index, 1);

    }

    private void OverrideAnimatorIfPossible()
    {
        if (!currentWeaponModel.TryGetComponent<Enemy_WeaponModel>(out var weaponComponent))
            return;

        AnimatorOverrideController animatorOverride = weaponComponent.animatorOverride;

        if (animatorOverride != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = animatorOverride;
        }
    }

    // It's important to notice that I'm setting global position and rotation, and in the player I used local.
    // This is because I had already configured the rig references positions to compensate the differences in local and global position.
    // Making both use local (the recommended) would take me a lot of time to do now.
    // TODO: Fix this.
    private void SetupRigIKConstrains(Transform target, Transform hint)
    {
        leftHandIKTarget.SetPositionAndRotation(target.position, target.rotation);
        leftHandIKHint.SetPositionAndRotation(hint.position, hint.rotation);
    }

    public void IKActive(bool leftHandIKActive, bool weaponAimIKActive, float changeRate = 10)
    {
        rigChangeRate = changeRate;
        leftHandIKTargetWeight = leftHandIKActive ? 0.8f : 0; // Max weight 0.8f
        weaponAimIKTargetWeight = weaponAimIKActive ? 0.5f : 0; // Max weight 0.5f, or the weapon won't move correctly,
                                                                // and the left hand will be off the correct position
    }

    private float UpdateIKWeights(float currentWeight, float targetWeight)
    {
        if (Mathf.Abs(currentWeight - targetWeight) > 0.05f)
            return Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * rigChangeRate);
        else
            return targetWeight;
    }
}
