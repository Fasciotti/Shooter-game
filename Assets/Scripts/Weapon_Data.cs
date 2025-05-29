using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data" )]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;


    [Header("Magazine Details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;


    [Header("Regular Shot")]
    public ShootType shootType;
    public float fireRate;
    public int bulletsPerShoot = 1;


    [Header("Burst Shot")]

    public bool burstAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;


    [Header("Spread")]

    public float baseSpread = 1;
    public float maxSpread = 3;
    public float spreadIncreaseRate = .15f;


    [Header("Weapon Generics")]
    
    [Range(1,3)]
    public float reloadSpeed = 1;
    [Range(1, 3)]
    public float equipSpeed = 1;
    [Range(4, 8)]
    public float weaponMaximumDistance = 4;
    [Range (4, 8)]
    public float cameraDistance = 6;

}
