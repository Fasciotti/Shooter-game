using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy Range/Weapon Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("Weapon Settings")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1; //Bullets per second
    
    public float minBulletsPerAttack = 1;
    public float maxBulletsPerAttack = 1;

    public float minWeaponCooldown = 1.5f;
    public float maxWeaponCooldown = 2.5f;

    [Header("Bullet Settings")]
    public float bulletSpeed = 20;
    public float weaponSpread = 0.1f;

    public float GetRandomBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack + 1);

    public float GetRandomWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);



    /// <summary>
    /// This returns the passed bullet direction with spread applied.
    /// </summary>
    public Vector3 ApplyWeaponSpread(Vector3 currentBulletDirection)
    {
        // Changing the direction of the bullet within the weaponSpread min-max range.
        Quaternion spreadDirection = Quaternion.Euler(
            Random.Range(-weaponSpread, weaponSpread),
            Random.Range(-weaponSpread, weaponSpread),
            Random.Range(-weaponSpread, weaponSpread)
            );

        // This applies the euler rotation to the vector direction.
        // (It's important to remember that this is not a multiplication operation,
        // but rather a custom operator to apply euler rotation. The order (Quaternion) & (Vector) is mandatory)
        return spreadDirection * currentBulletDirection;
    }
}
