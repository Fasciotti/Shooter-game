using System.Runtime.CompilerServices;
using UnityEngine;

public enum WeaponType
{
    pistol,
    autoRifle,
    shotgun,
    revolver,
    rifle
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
    public int maganizeCapacity;
    public int totalReserveAmmo;

    [Header("Shooting specifics")]
    public float fireRate = 1; // Bullets per second
    private float lastShootTime;
    public ShootType shootType;


    [Header("Animation Details")]
    // Controls reload and equip animation speeds
    [Range(1, 2)]
    public float equipSpeed = 1;
    [Range(1, 2)]
    public float reloadSpeed = 1;




    #region Shoot methods
    public bool CanShoot()
    {
        if(HaveEnoughBulletsToShoot() && IsReadyToShot())
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }
    private bool IsReadyToShot()
    {
        if (Time.time >= lastShootTime + (1 / fireRate))
        {
            lastShootTime = Time.time;
            return true;

        }

        return false;
    }

    #endregion

    #region Reload methods
    public bool CanReload()
    {
        if (bulletsInMagazine == maganizeCapacity)
            return false;

        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }
    private bool HaveEnoughBulletsToShoot() => bulletsInMagazine > 0;

    // It's called to refill at the end of the reload animation
    public void RefillBullets()
    {
        //totalReserveAmmo += bulletsInMagazine; This can be used to maintain the bullets in the magazine after reloading

        int bulletsToReload = maganizeCapacity;

        if (bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }

    #endregion
}
