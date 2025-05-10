using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20f;       //this is the default speed from which the mass of the bullet is calculated

    private Player player;
    [SerializeField] private Weapon currentWeapon;

    [Header("Bullet options")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;


    private void Start()
    {
        player = GetComponent<Player>();

        ShootInput();

        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    private void ShootInput()
    {
        player.controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        if (currentWeapon.ammo <= 0)
        {
            Debug.Log("No more bullets");
            return;
        }

        currentWeapon.ammo--;

        GameObject newBullet = 
            Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();

        newBulletRb.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        newBulletRb.linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.AimTransform();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (!player.aim.CanAimPrecisely() && player.aim.Target() == null)
            direction.y = 0;

        //gunPoint.LookAt(aim);
        //weaponHolder.LookAt(aim); TODO: find a better place for this;

        return direction;
    }

    public Transform GunPoint() => gunPoint;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
        Gizmos.color = Color.yellow;

        //Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    }
}
