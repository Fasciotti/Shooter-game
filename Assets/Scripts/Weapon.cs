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

    [Header("Magazine Details")]
    public int bulletsInMagazine;
    public int totalReserveAmmo;
    public int magazineCapacity;

    // Controls reload and equip animation speeds
    [Range(1, 2)]
    public float equipSpeed = 1;
    [Range(1, 2)]
    public float reloadSpeed = 1;
    [Range(2, 12)]
    public float gunDistance = 4;

    [Header("Shooting specifics")]
    public float fireRate = 1; // fireRate represents shots per second
    public float defaultFireRate = 1f;
    public int bulletsPerShoot;
    public ShootType shootType;
    private float lastShootTime;

    [Header("Burst Fire")]
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay;

    [SerializeField] private bool isBurstAvailable;
    [SerializeField] private bool isBurstActivated;

    [Header("Spread")]
    private float currentSpread = 1;
    private float lastSpreadTimeUpdate;

    public float baseSpread = 1;
    public float maximumSpread = 3;
    public float spreadCooldown = 1; // In seconds
    public float spreadIncreaseRate = 0.15f; // Per shot


    #region BurstMode methods
    public void ToogleBurst()
    {
        if (!isBurstAvailable)
            return;

        isBurstActivated = !isBurstActivated;

        UpdateShootSpecifics();
    }

    private void UpdateShootSpecifics()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            isBurstActivated = true;
        }

        if (isBurstActivated)
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
    public bool IsBurstActivated() => isBurstActivated;

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

        // This applies the euler rotation to the vector position.
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
