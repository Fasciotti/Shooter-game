using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private Player player;

    [SerializeField] private Weapon_Data weaponData;


    public override void Interaction()
    {
        base.Interaction();

        player.weapon.PickUpWeapon(weaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (player == null)
        {
            player = other.GetComponent<Player>();
        }
    }
}
