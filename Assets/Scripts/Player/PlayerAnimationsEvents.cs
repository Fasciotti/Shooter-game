using UnityEngine;
using UnityEngine.Android;

public class PlayerAnimationsEvents : MonoBehaviour
{
    // TODO: Separate the rigging and IK resets in methods

    private Player player => GetComponentInParent<Player>();
    public void ReloadIsOver()
    {
        player.weapon.CurrentWeapon().RefillBullets();
        player.weapon.SetWeaponReady(true);
        Debug.Log("ready");

    }

    public void ResetRigConstraints()
    {
        player.weaponVisuals.RigWeightReset();
        player.weaponVisuals.LHandIKWeightReset();
        Debug.Log("reset");
    }
    public void EquipWeaponIsOver()
    {
        player.weapon.SetWeaponReady(true);
        Debug.Log("ready");
    }

    public void SwitchOnWeaponModel() => player.weaponVisuals.SwitchOnCurrentWeaponModel();
}
