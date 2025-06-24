using UnityEngine;
public class PlayerAnimationsEvents : MonoBehaviour
{
    // TODO: Separate the rigging and IK resets in methods

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void ReloadIsOver()
    {
        player.weapon.CurrentWeapon().RefillBullets();
        player.weapon.SetWeaponReady(true);
    }

    public void ResetRigConstraints()
    {
        player.weaponVisuals.RigWeightReset();
        player.weaponVisuals.LHandIKWeightReset();
    }
    public void EquipWeaponIsOver()
    {
        player.weapon.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => player.weaponVisuals.SwitchOnCurrentWeaponModel();
}
