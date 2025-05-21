using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f;       // This is the default speed from which the mass of the bullet is calculated

    private Player player;
    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

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
        currentWeapon.bulletsInMagazine = currentWeapon.magazineCapacity;
    }

    private void Update()
    {
        if (isShooting)
            Shoot();

        if (Input.GetKeyDown(KeyCode.T))
            currentWeapon.ToogleBurst();
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
        if (!IsWeaponReady() || !currentWeapon.CanShoot())
            return;

        // Assigning false to isShooting variable makes the chain of calls of Shoot methods stop (see Update)
        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        player.weaponVisuals.PlayFireAnimation();

        if (currentWeapon.IsBurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
    }

    // FIXME: Because of the weaponReady variable, the laser is disabled when shooting with burst mode on
    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);

        for (int i = 1; i <= currentWeapon.bulletsPerShoot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShoot)
            {
                SetWeaponReady(true);
            }

        }
    }


    private void FireSingleBullet()
    {
        GameObject newBullet = ObjectPool.instance.GetBullet();

        // Makes the bullet face aim
        newBullet.transform.SetPositionAndRotation
            (CurrentWeaponGunPoint().position, Quaternion.LookRotation(CurrentWeaponGunPoint().forward));

    

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());


        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();
        Bullet bullet = newBullet.GetComponent<Bullet>();


        bullet.BulletSetup(currentWeapon.gunDistance);
        bullet.ClearTrail();

        newBulletRb.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // This makes sure the mass of the bullet is always the same
        newBulletRb.linearVelocity = bulletDirection * bulletSpeed;

        // Decreases ammo
        currentWeapon.bulletsInMagazine--;

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

        controls.Character.Fire.performed += _ => isShooting = true;
        controls.Character.Fire.canceled += _ => isShooting = false;

        controls.Character.EquipWeapon1.performed += _ => EquipWeapon(0);
        controls.Character.EquipWeapon2.performed += _ => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += _ => DropWeapon();

        controls.Character.Reload.performed += _ => Reload();
    }

    #endregion
}