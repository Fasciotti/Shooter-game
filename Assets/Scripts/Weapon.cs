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

    public bool canShoot()
    {
        return HaveEnoughBullets();
    }

    // This also removes one bullet from the weapon, is called when shooting.
    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    public bool canReload()
    {
        if (bulletsInMagazine == maganizeCapacity)
            return false;

        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }

    public void refillBullets()
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
}
