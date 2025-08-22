using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    AutoRifle,
    Shotgun,
    Revolver,
    Rifle
}

public enum ShootType
{
    Single,
    Auto
}

[System.Serializable] // Makes class visible on inspector
public class Weapon
{

    public WeaponType weaponType;

    [Header("Bullet")]
    public int bulletDamage;


    [Header("Magazine Details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;


    #region Regular Mode Variables
    [Header("Regular Shot")]

    public ShootType shootType;

    public float fireRate; // fireRate represents shots per second
    private float defaultFireRate;
    private float lastShootTime;
    public int bulletsPerShoot { get; private set; }
    #endregion

    #region Burst Mode Variables
    [Header("Burst")]
    public bool burstActive;
    private bool burstAvailable;

    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay { get; private set; }
    #endregion

    #region Spread Variables
    [Header("Spread")]
    private float currentSpread = 1;
    private float lastSpreadTimeUpdate;

    private float baseSpread;
    private float maximumSpread;
    private float spreadCooldown = 1; // In seconds
    private float spreadIncreaseRate; // Per shot
    #endregion

    #region Weapon Generic Variables
    // Controls reload and equip animation speeds
    public float equipSpeed {  get; private set; }
    public float reloadSpeed { get; private set; }
    public float weaponMaximumDistance {  get; private set; }
    public float cameraDistance {  get; private set; }
    #endregion

    public Weapon_Data weaponData { get; private set; } // Default weaponData


    public Weapon(Weapon_Data weaponData)
    {
        //Bullet
        bulletDamage = weaponData.bulletDamage;


        // Magazine Details
        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;


        // Regular
        shootType = weaponData.shootType;
        fireRate = weaponData.fireRate;
        bulletsPerShoot = weaponData.bulletsPerShoot;

        defaultFireRate = fireRate;


        // Burst
        burstAvailable = weaponData.burstAvailable;
        burstActive = weaponData.burstActive;

        burstBulletsPerShot = weaponData.burstBulletsPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;


        // Spread
        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maxSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;


        // Generics
        weaponType = weaponData.weaponType;
        reloadSpeed = weaponData.reloadSpeed;
        equipSpeed = weaponData.equipSpeed;
        weaponMaximumDistance = weaponData.weaponMaximumDistance;
        cameraDistance = weaponData.cameraDistance;

        // WeaponData
        this.weaponData = weaponData;
    }


    #region BurstMode methods
    public void ToogleBurst()
    {
        if (!burstAvailable)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShoot = burstBulletsPerShot;
            fireRate = burstFireRate;

        }
        else
        {
            bulletsPerShoot = 1;
            fireRate = defaultFireRate;
        }
    }
    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    #endregion

    #region Spread methods

    public Vector3 ApplySpread(Vector3 currentBulletDirection)
    {
        UpdateSpread();

        // Changing the direction of the bullet within the currentSpread min-max range.
        Quaternion spreadDirection = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread)
            );

        // This applies the euler rotation to the vector direction.
        // (It's important to remember that this is not a multiplication operation,
        // but rather a custom operator to apply euler rotation. The order (Quaternion) & (Vector) is mandatory)
        return spreadDirection * currentBulletDirection;
    }


    public void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    public void UpdateSpread()
    {
        // This checks if the required time (spreadCooldown) has passed since the last time the player shot. If so, the spread is reset.
        if (Time.time >  lastSpreadTimeUpdate + spreadCooldown)
        {
            currentSpread = baseSpread;
        } else
        {
            IncreaseSpread();
        }

        lastSpreadTimeUpdate = Time.time;
    }

    #endregion

    #region Shoot methods
    public bool CanShoot() => HaveEnoughBulletsToShoot() && IsReadyToShot();
    private bool IsReadyToShot()
    {
        // Player can only shoot when the minimum time has passed. (fireRate represents shots per second.
        // The time between bullets is 1 / fireRate seconds.)
        if (Time.time >= lastShootTime + (1 / fireRate))
        {
            lastShootTime = Time.time;
            return true;

        }

        return false;
    }

    #endregion

    #region Reload methods

    // Can only reload if it has ammo and it's not full.
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;

        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }
    private bool HaveEnoughBulletsToShoot() => bulletsInMagazine > 0;

    // It's called to refill at the end of the reload animation in PlayerAnimationsEvents.
    public void RefillBullets()
    {
        // This can be used to maintain the bullets in the magazine after reloading.
        // totalReserveAmmo += bulletsInMagazine;

        int bulletsToReload = magazineCapacity;

        // Fill up the magazine with the remaining reserve
        if (bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }

    #endregion
}
