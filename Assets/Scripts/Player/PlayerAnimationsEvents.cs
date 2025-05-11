using UnityEngine;

public class PlayerAnimationsEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
    }
    public void ReloadIsOver()
    {
        visualController.RigWeightReset();
        Debug.Log("test1");
    }

    public void GrabWeaponIsOver()
    {
        Debug.Log("test");
        visualController.NotBusyGrabbingWeapon();
    }

    public void GrabWeaponFinishAnimation()
    {
        visualController.LHandIKWeightReset();
        visualController.RigWeightReset();
    }
}
