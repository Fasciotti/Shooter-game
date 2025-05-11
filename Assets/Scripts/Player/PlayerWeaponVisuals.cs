using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerWeaponVisuals : MonoBehaviour
{
    /* =====================
     *   Inspector Fields
     * ===================== */

    // Weapon transforms (0‑4 match enum in comment)
    [SerializeField] private Transform[] gunTransforms;

    /* 0 = pistol (layer1)
     * 1 = autoRifle (layer1)
     * 2 = shotgun  (layer2)
     * 3 = revolver (layer1)
     * 4 = rifle    (layer3)
     */

    [Header("Rig")]
    [SerializeField] private float rigWeightChangeRate;   // Speed of rig weight fade

    [Header("LeftHandIK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;

    [SerializeField] private Transform leftHandTarget;    
    [SerializeField] private float LHandIKWeightChangeRate;    // Speed of LeftHand IK weight fade

    /* =====================
     *   Private State
     * ===================== */

    private Transform currentGun;        // Currently active weapon
    private Animator anim;
    private Player player;

    private bool shouldRigWeightIncrease;
    private bool shouldLHandIKWeightIncrease;
    private Rig rig;

    /* =====================
     *   Unity Messages
     * ===================== */

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        AssingWeaponControlEvents();
    }

    private void Update()
    {
        MaximizeRigWeight();
        MaximizeLeftHandIKWeight();
    }

    /* =====================
     *   Input / Events
     * ===================== */

    private void AssingWeaponControlEvents()
    {
        // Weapon switching
        //player.controls.Character.EquipWeapon.performed += context => CheckWeaponSwitch(context);

        // Reload action
        player.controls.Character.Reload.performed += context =>
        {
            if (!(anim.GetBool("BusyGrabbingWeapon")) && !(anim.GetBool("Reload")))
            {
                Debug.Log("Reloading");
                anim.SetTrigger("Reload");
                ReduceRigWeight();
            }
            else
            {
                anim.ResetTrigger("Reload");
            }
        };
    }

    private void CheckWeaponSwitch(InputAction.CallbackContext context)
    {
        switch (context.control.displayName)
        {
            case "1": // pistol
                SwitchOn(gunTransforms[0]);
                SwitchAnimationLayer(1);
                WeaponGrabAnimation(GrabType.sideGrab);
                break;

            case "2": // autoRifle
                SwitchOn(gunTransforms[1]);
                SwitchAnimationLayer(1);
                WeaponGrabAnimation(GrabType.sideGrab);
                break;

            case "3": // shotgun
                SwitchOn(gunTransforms[2]);
                SwitchAnimationLayer(2);
                WeaponGrabAnimation(GrabType.backGrab);
                break;

            case "4": // revolver
                SwitchOn(gunTransforms[3]);
                SwitchAnimationLayer(1);
                WeaponGrabAnimation(GrabType.sideGrab);
                break;

            case "5": // rifle
                SwitchOn(gunTransforms[4]);
                SwitchAnimationLayer(3);
                WeaponGrabAnimation(GrabType.backGrab);
                break;

            default:
                break;
        }
    }

    /* =====================
     *   Rig & IK Control
     * ===================== */

    private void ReduceRigWeight() => rig.weight = 0.15f;          // Briefly lower rig during transitions

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

    /* =====================
     *   Weapon Handling
     * ===================== */

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        // Align left‑hand target to weapon‑specific anchor
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;
        leftHandTarget.localScale = targetTransform.localScale;
    }

    private void WeaponGrabAnimation(GrabType grabType)
    {
        ReduceRigWeight();
        ReduceLHandIKWeight();
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");
        anim.SetBool("BusyGrabbingWeapon", true);
    }

    public void NotBusyGrabbingWeapon() => anim.SetBool("BusyGrabbingWeapon", false);

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0);

        anim.SetLayerWeight(layerIndex, 1);
    }

    public enum GrabType
    { sideGrab, backGrab };
}