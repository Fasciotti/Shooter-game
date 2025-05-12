using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;
    [SerializeField] private int maxSlots = 2;



    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();

        currentWeapon = weaponSlots[0];
        currentWeapon.bulletsInMagazine = currentWeapon.maganizeCapacity;
    }


    private void Shoot()
    {
        if (!currentWeapon.canShoot())
        {
            return;
        }

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

    private void EquipWeapon(int i)
    {
        if (i <= weaponSlots.Count - 1)
            currentWeapon = weaponSlots[i];
    }

    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1)
            return;

        weaponSlots.Remove(currentWeapon);
        currentWeapon = weaponSlots[0];
    }

    public void PickUpWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            Debug.Log("Cannot drop weapon");
            return;
        }

        weaponSlots.Add(newWeapon);
    }

    public Transform GunPoint() => gunPoint;
    public Weapon CurrentWeapon() => currentWeapon;

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += _ => Shoot();

        controls.Character.EquipWeapon1.performed += _ => EquipWeapon(0);
        controls.Character.EquipWeapon2.performed += _ => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += _ => DropWeapon();

        controls.Character.Reload.performed += _ =>
        {
            if(currentWeapon.canReload())
                player.weaponVisuals.PlayAnimationReload();
        };       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);
        Gizmos.color = Color.yellow;

        //Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
    }
}
