using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weapon);
    }
}
