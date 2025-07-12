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

    [SerializeField] Weapon_Data defaultWeaponData;

    [Header("Bullet options")]
    [SerializeField] private float bulletImpactForce = 100;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    [SerializeField] private GameObject weaponPickupPrefab;

    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();

        //Used invoke to make sure all data has been loaded.
        Invoke("EquipStartingWeapon", 0.1f);

    }

    private void Update()
    {
        if (isShooting)
            Shoot();

    }


    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);

        EquipWeapon(0);
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
        if (i >= weaponSlots.Count)
            return;

        // This prevents the player from getting the same newWeapon
        if (currentWeapon == weaponSlots[i])
            return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i]; // Main purpose

        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);

        player.weaponVisuals.PlayWeaponEquipAnimation();

    }

    // Assumes maximum of 2 slots
    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
            return;

        player.weaponVisuals.SwitchOffWeaponModels();
        CreateWeaponOnGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    private void CreateWeaponOnGround()
    {
        GameObject droppedWeaponPickup = ObjectPool.Instance.GetObject(weaponPickupPrefab);

        droppedWeaponPickup.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    // Called by the pickup object
    public void PickUpWeapon(Weapon Weapon)
    {

        // This verifies if the player already has this newWeapon in the inventory, if so, add ammo to it.
        if (WeaponInSlots(Weapon.weaponType) != null)
        {
            Debug.Log("You already have this weapon equipped. Collecting ammo...");

            WeaponInSlots(Weapon.weaponType).totalReserveAmmo += Weapon.bulletsInMagazine;

            return;
        }

        // This verifies if the player has a full inventory and is not trying to get the same weapon he is holding (though redundant), if not, replace it
        if (weaponSlots.Count >= maxSlots && currentWeapon.weaponType != Weapon.weaponType)
        {
            Debug.Log("Inventory is full. Replacing the weapon...");

            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisuals.SwitchOffWeaponModels();
            CreateWeaponOnGround();

            weaponSlots[weaponIndex] = Weapon;
            EquipWeapon(weaponIndex);

            return;
        }

        weaponSlots.Add(Weapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel(); // Makes the picked newWeapon appear 
    }


    #region Lambda Methods
    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool IsWeaponReady() => weaponReady;

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    #endregion
    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (var weapon in weaponSlots)
        {
            if (weaponType == weapon.weaponType)
                return weapon;
        }

        return null;
    }

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

        if (currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }

        PlayerShootingEnemy();
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

            if (currentWeapon.bulletsInMagazine <= 0 )
            {
                SetWeaponReady(true);
                yield break;
            }

            if (i >= currentWeapon.bulletsPerShoot)
            {
                SetWeaponReady(true);
            }

        }
    }


    private void FireSingleBullet()
    {
        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);

        // Makes the bullet face aim
        newBullet.transform.SetPositionAndRotation
            (CurrentWeaponGunPoint().position, Quaternion.LookRotation(CurrentWeaponGunPoint().forward));


        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());


        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();
        Bullet bullet = newBullet.GetComponent<Bullet>();


        bullet.BulletSetup(currentWeapon.weaponMaximumDistance, bulletImpactForce);

        newBulletRb.mass = REFERENCE_BULLET_SPEED / bulletSpeed; // This makes sure the mass of the bullet is always the same
        newBulletRb.linearVelocity = bulletDirection * bulletSpeed;

        // Decreases ammo
        currentWeapon.bulletsInMagazine--;

    }

    public Weapon BackupWeaponModel()
    {
        // The backupWeapon is defined as the newWeapon the player doesn't currently have equipped, but it's in the weaponSlots.
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

    public void PlayerShootingEnemy()
    {
        Vector3 rayOrigin = CurrentWeaponGunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Enemy_Melee enemy = hitInfo.collider.gameObject.GetComponentInParent<Enemy_Melee>();

            if (enemy != null)
            {
                enemy.ActivateDodgeAnimation();
            }
        }
    }

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += _ => isShooting = true;
        controls.Character.Fire.canceled += _ => isShooting = false;

        controls.Character.EquipWeapon1.performed += _ => EquipWeapon(0);
        controls.Character.EquipWeapon2.performed += _ => EquipWeapon(1);
        controls.Character.EquipWeapon3.performed += _ => EquipWeapon(2);
        controls.Character.EquipWeapon4.performed += _ => EquipWeapon(3);
        controls.Character.EquipWeapon5.performed += _ => EquipWeapon(4);

        controls.Character.ToogleBurst.performed += _ => currentWeapon.ToogleBurst();

        controls.Character.DropCurrentWeapon.performed += _ => DropWeapon();

        controls.Character.Reload.performed += _ => Reload();
    }

    #endregion
}