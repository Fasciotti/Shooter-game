using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerWeaponVisuals : MonoBehaviour
{
    /* =====================
     *   Inspector Fields
     * ===================== */

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightChangeRate;   // Speed of rig weight fade

    [Header("LeftHandIK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;

    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private float LHandIKWeightChangeRate;    // Speed of LeftHand IK weight fade

    /* =====================
     *   Private State
     * ===================== */

    private Animator anim;
    private Player player;

    private bool shouldRigWeightIncrease;
    private bool shouldLHandIKWeightIncrease;
    private Rig rig;
    private bool isGrabbingWeapon;

    /* =====================
     *   Unity Messages
     * ===================== */

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        MaximizeRigWeight();
        MaximizeLeftHandIKWeight();
    }

    public void PlayAnimationReload()
    {
        if (!(anim.GetBool("BusyGrabbingWeapon")) && !(anim.GetBool("Reload")))
        {
            Debug.Log("Reloading");
            anim.SetTrigger("Reload");
            ReduceRigWeight();
        }
        else
        {
            // This prevents the reload animation to play after the current animation ends if the button is clicked while doing so.
            anim.ResetTrigger("Reload");
        }
    }


    public WeaponModel currentWeaponModel()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        foreach (var i in weaponModels)
        {
            if (i.weaponType == weaponType)
                weaponModel =  i;
        }

        return weaponModel;
    }


    #region Rig Animations Methods
    private void ReduceRigWeight() => rig.weight = 0.2f;          // Briefly lower rig during transitions

    private void MaximizeRigWeight()
    {
        if (shouldRigWeightIncrease)
        {
            rig.weight += rigWeightChangeRate * Time.deltaTime;
            if (rig.weight >= 1) shouldRigWeightIncrease = false;
        }
    }

    public void RigWeightReset() => shouldRigWeightIncrease = true;

    private void ReduceLHandIKWeight() => leftHandIKConstraint.weight = 0f;

    private void MaximizeLeftHandIKWeight()
    {
        if (shouldLHandIKWeightIncrease)
        {
            leftHandIKConstraint.weight += LHandIKWeightChangeRate * Time.deltaTime;
            if (leftHandIKConstraint.weight >= 1) shouldLHandIKWeightIncrease = false;
        }
    }

    public void LHandIKWeightReset() => shouldLHandIKWeightIncrease = true;
    private void AttachLeftHand()
    {
        // Align left‑hand target to weapon‑specific anchor
        Transform targetTransform = currentWeaponModel().holdPoint;

        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;
        leftHandTarget.localScale = targetTransform.localScale;
    }

    #endregion

    public void SwitchOnCurrentWeaponModel()
    {
        int animationLayerIndex = ((int)currentWeaponModel().holdType);

        SwitchOffBackupWeaponModels();
        SwitchOffWeaponModels();

        currentWeaponModel().gameObject.SetActive(true); // <-- Main purpose of this method
        if (!player.weapon.HasOnlyOneWeapon())
            SwitchOnBackupWeaponModel();

        SwitchAnimationLayer(animationLayerIndex);
        AttachLeftHand();
    }
    public void SwitchOffWeaponModels()
    {
        foreach (WeaponModel weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }
    }

    public void SwitchOffBackupWeaponModels()
    {
        // Every backup weapon model has a BackupWeaponModel script, we search for it and turn all of them off.
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.gameObject.SetActive(false);
        }

    }
    public void SwitchOnBackupWeaponModel()
    {
        // We compare the backupWeaponType (defined in PlayerWeaponController) with all of the models to decide which to turn on.
        // The backupWeapon is defined as the weapon the player doesn't currently have in the weaponSlots.
        WeaponType backupWeaponType = player.weapon.BackupWeaponModel().weaponType;

        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            if(backupModel.weaponType == backupWeaponType)
            {
                backupModel.gameObject.SetActive(true);
            }
        }

    }


    public void PlayWeaponEquipAnimation()
    {
        GrabType grabType = currentWeaponModel().grabType;

        ReduceRigWeight();
        ReduceLHandIKWeight();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab"); // calls SwitchOnWeaponModel in PlayerAnimationsEvents
        SetBusyGrabWeaponTo(true);
    }

    public void SetBusyGrabWeaponTo(bool busy)
    {
        isGrabbingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0);

        anim.SetLayerWeight(layerIndex, 1);
    }

}