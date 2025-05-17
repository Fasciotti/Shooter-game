using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f;       // This is the default speed from which the mass of the bullet is calculated

    private Player player;
    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;

    [Header("Bullet options")]
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;
    [SerializeField] private int maxSlots = 2;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();

        currentWeapon = weaponSlots[0];
        currentWeapon.bulletsInMagazine = currentWeapon.maganizeCapacity;
    }


    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.AimTransform();

        Vector3 direction = (aim.position - CurrentWeaponGunPoint().position).normalized;

        if (!player.aim.CanAimPrecisely() && player.aim.Target() == null)
            direction.y = 0;

        return direction;
    }


    #region ============ Slot Management ==============
    private void EquipWeapon(int i)
    {
        // This prevents outofscope exception
        if (i > weaponSlots.Count - 1)
            return;

        // This prevents the player from getting the same weapon
        if (currentWeapon == weaponSlots[i])
            return;

        SetWeaponReady(false);
        
        currentWeapon = weaponSlots[i];

        player.weaponVisuals.PlayWeaponEquipAnimation();
    }

    // Assumes maximum of 2 slots
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
            return;

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    // Called by the pickup object
    public void PickUpWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            Debug.Log("Cannot drop weapon");
            return;
        }

        // This verifies if the player already has this weapon in the inventory
        if (weaponSlots[0].weaponType == newWeapon.weaponType)
            return;

        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel(); // Makes the picked weapon appear 
    }


    #region Lambda Methods
    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool IsWeaponReady() => weaponReady;

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    #endregion
    #endregion
    private void Reload()
    {
        if (!IsWeaponReady())
            return;

        if (!currentWeapon.CanReload())
            return;
        
        SetWeaponReady(false);
        player.weaponVisuals.PlayAnimationReload();
    }
    private void Shoot()
    {
        if (!IsWeaponReady())
            return;

        if (!currentWeapon.CanShoot())
            return;

        GameObject newBullet = ObjectPool.instance.GetBullet();

        newBullet.transform.SetPositionAndRotation
            (CurrentWeaponGunPoint().position, Quaternion.LookRotation(CurrentWeaponGunPoint().forward));

        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();

        newBulletRb.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // This makes sure the mass of the bullet is always the same
        newBulletRb.linearVelocity = BulletDirection() * bulletSpeed;

        player.weaponVisuals.PlayFireAnimation();
    }

    public Weapon BackupWeaponModel()
    {
        // The backupWeapon is defined as the weapon the player doesn't currently have equipped, but it's in the weaponSlots.
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;
        }
        Debug.Log("Backup model not available");
        return null;
    }
    #region Lambda Methods
    public Transform CurrentWeaponGunPoint() => player.weaponVisuals.currentWeaponModel().gunPoint;

    public Weapon CurrentWeapon() => currentWeapon;

    #endregion

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += _ => Shoot();

        controls.Character.EquipWeapon1.performed += _ => EquipWeapon(0);
        controls.Character.EquipWeapon2.performed += _ => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += _ => DropWeapon();

        controls.Character.Reload.performed += _ => Reload();
    }

    #endregion
}