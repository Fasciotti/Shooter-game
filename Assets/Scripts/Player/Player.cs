using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls {  get; private set; }
    public PlayerMovement movement {  get; private set; }
    public PlayerAim aim {  get; private set; }
    public PlayerWeaponController weapon {  get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }

    private void OnEnable()
    {
        controls = new PlayerControls();
        controls.Enable();
        movement = GetComponent<PlayerMovement>();
        aim = GetComponent<PlayerAim>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
