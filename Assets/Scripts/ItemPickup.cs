using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] private Weapon_Data weaponData;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weaponData);
    }
}
