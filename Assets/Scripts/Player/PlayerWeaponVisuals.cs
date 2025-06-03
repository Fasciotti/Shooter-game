using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightChangeRate;   // Speed of rig weight fade

    [Header("LeftHandIK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;

    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private float LHandIKWeightChangeRate;    // Speed of LeftHand IK weight fade

    public Animator anim { get; private set; }
    private Player player;

    private bool shouldRigWeightIncrease;
    private bool shouldLHandIKWeightIncrease;
    private Rig rig;
    private bool isEquippingWeapon;

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
    private void ReduceLHandIKWeight() => leftHandIKConstraint.weight = 0f;

    // TODO: Merge rig and IK weight-changing methods. Make methods stops increasing weight.
    private void MaximizeRigWeight()
    {
        if (shouldRigWeightIncrease)
        {
            rig.weight += rigWeightChangeRate * Time.deltaTime;
            if (rig.weight >= 1) shouldRigWeightIncrease = false;
        }
    }

    private void MaximizeLeftHandIKWeight()
    {
        if (shouldLHandIKWeightIncrease)
        {
            leftHandIKConstraint.weight += LHandIKWeightChangeRate * Time.deltaTime;
            if (leftHandIKConstraint.weight >= 1) shouldLHandIKWeightIncrease = false;
        }
    }


    public void RigWeightReset() => shouldRigWeightIncrease = true;
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

    public void SwitchOffWeaponModels()
    {
        foreach (WeaponModel weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }
    }
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

    public void SwitchOffBackupWeaponModels()
    {
        // Every backup weapon model has a BackupWeaponModel script, we search for it and turn them all off.
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.ActivateModel(false);
        }

    }

    // The backup weapon is defined as the weapon the player does have in the weaponSlots, but it's not equipped.
    public void SwitchOnBackupWeaponModel()
    {

        SwitchOffBackupWeaponModels();

        BackupWeaponModel lowHangWeapon = null, backHangWeapon = null, sideHangWeapon = null;


        foreach (BackupWeaponModel backupModel in backupWeaponModels)
        {
            if (player.weapon.CurrentWeapon().weaponType == backupModel.weaponType)
                continue;

            if (player.weapon.WeaponInSlots(backupModel.weaponType) != null)
            {
                if (backupModel.HangTypeEquals(HangType.LowBackHang))
                {
                    lowHangWeapon = backupModel;
                }

                if (backupModel.HangTypeEquals(HangType.BackHang))
                {
                    backHangWeapon = backupModel;
                }

                if (backupModel.HangTypeEquals(HangType.SideHang))
                {
                    sideHangWeapon= backupModel;
                }

            }


        }

        lowHangWeapon?.ActivateModel(true);
        backHangWeapon?.ActivateModel(true);
        sideHangWeapon?.ActivateModel(true);

    }

    public void PlayFireAnimation()
    {
        anim.SetTrigger("Fire");
    }

    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = currentWeaponModel().equipType;

        ReduceRigWeight();
        ReduceLHandIKWeight();

        float equipSpeed = player.weapon.CurrentWeapon().equipSpeed;

        anim.SetFloat("EquipType", (float)equipType);
        anim.SetFloat("EquipSpeed", equipSpeed);

        anim.SetTrigger("EquipWeapon"); // calls SwitchOnWeaponModel in PlayerAnimationsEvents
        player.weapon.SetWeaponReady(false);
    }
    public void PlayAnimationReload()
    {
        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        Debug.Log("Reloading");
        anim.SetFloat("ReloadSpeed", reloadSpeed);
        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0);

        anim.SetLayerWeight(layerIndex, 1);
    }

}