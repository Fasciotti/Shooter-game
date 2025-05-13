using UnityEngine;

public class PlayerAnimationsEvents : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    public void ReloadIsOver()
    {
        player.weaponVisuals.RigWeightReset();
        player.weapon.CurrentWeapon().refillBullets();
    }

    public void GrabWeaponIsOver()
    {
        player.weaponVisuals.SetBusyGrabWeaponTo(false);
        player.weaponVisuals.LHandIKWeightReset();
        player.weaponVisuals.RigWeightReset();
    }

    public void GrabWeaponFinishAnimation()
    {
    }

    public void SwitchOnWeaponModel() => player.weaponVisuals.SwitchOnCurrentWeaponModel();
}
