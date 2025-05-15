using UnityEngine;

public class PlayerAnimationsEvents : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    public void ReloadIsOver()
    {
        player.weaponVisuals.RigWeightReset();
        player.weapon.CurrentWeapon().refillBullets();
    }

    public void EquipWeaponIsOver()
    {
        player.weaponVisuals.SetBusyEquippingWeaponTo(false);
        player.weaponVisuals.LHandIKWeightReset();
        player.weaponVisuals.RigWeightReset();
    }

    public void SwitchOnWeaponModel() => player.weaponVisuals.SwitchOnCurrentWeaponModel();
}
