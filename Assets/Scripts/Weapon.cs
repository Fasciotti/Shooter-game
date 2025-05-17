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

[System.Serializable] // Makes class visible on inspector
public class Weapon
{
    public WeaponType weaponType;

    public int bulletsInMagazine;
    public int maganizeCapacity;
    public int totalReserveAmmo;


    // Controls reload and equip animation speeds
    [Range(1, 2)]
    public float equipSpeed = 1;
    [Range(1, 2)]
    public float reloadSpeed = 1;

    [Space]
    public float fireRate = 1; // Bullets per second

    private float lastShootTime;

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
