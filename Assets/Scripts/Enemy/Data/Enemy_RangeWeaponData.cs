using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy Range/Weapon Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1; //Bullets per second
    
    [SerializeField] private float minBulletsPerAttack = 1;
    [SerializeField] private float maxBulletsPerAttack = 1;

    [SerializeField] private float minWeaponCooldown = 1.5f;
    [SerializeField] private float maxWeaponCooldown = 2.5f;

    public float bulletSpeed = 20;
    public float weaponSpread = 0.1f;

    public float GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack);

    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);


}
