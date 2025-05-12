using UnityEngine;

public class PlayerAnimationsEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private Player player => GetComponentInParent<Player>();
    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
    }
    public void ReloadIsOver()
    {
        visualController.RigWeightReset();
        player.weapon.CurrentWeapon().refillBullets();
    }

    public void GrabWeaponIsOver()
    {
        visualController.NotBusyGrabbingWeapon();
    }

    public void GrabWeaponFinishAnimation()
    {
        visualController.LHandIKWeightReset();
        visualController.RigWeightReset();
    }
}
