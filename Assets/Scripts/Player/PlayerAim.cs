using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAim : MonoBehaviour
{
    #region ─── Runtime References ────────────────────────────────────────────────
    private Player player;          // Cached reference to the Player component
    #endregion

    #region ─── Input ─────────────────────────────────────────────────────────────
    private Vector2 mouseInput;              // Raw mouse coordinates (screen space)
    private float cameraZoom;
    #endregion

    #region ─── Inspector Fields ─────────────────────────────────────────────────

    [Header("Aim Visuals - Laser")]
    [SerializeField] private LineRenderer aimLaser;          // LineRenderer on the WeaponHolder

    [Header("Aim Control")]
    [SerializeField] private Transform aim;                 // World-space helper that other systems follow
    [SerializeField] private bool isAimingPrecise;
    [SerializeField] private bool isLockingToTarget;

    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;         // Follow target for the camera rig
    [Range(1.5f, 3f)][SerializeField] private float maxCameraDistance;
    [Range(.5f, 1.5f)][SerializeField] private float minCameraDistance;
    [Range(3f, 10f)][SerializeField] private float cameraSensitivity;
    float CameraDistance;

    [Space]
    [SerializeField] private LayerMask aimLayerMask;         // Layers hit by aiming raycasts
    #endregion

    #region ─── Internal State ───────────────────────────────────────────────────
    private RaycastHit lastKnownMouseHit;    // Stores the last valid hit so that we always have a value
    #endregion


    #region ─── Unity Lifecycle ──────────────────────────────────────────────────
    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();                 // Subscribes to input-action callbacks
    }

    private void Update()
    {
        UpdateAimPosition();                 // Position the aim helper
        UpdateCameraPosition();              // Move the camera target smoothly
        UpdateAimVisuals();                  // Update the laser LineRenderer
    }
    #endregion


    #region ─── Public Helpers ───────────────────────────────────────────────────
    public bool CanAimPrecisely() => isAimingPrecise;
    public Transform AimTransform() => aim;

    /// <summary>
    /// Returns the transform underneath the cursor **only** if it carries a <c>Target</c> component.
    /// Otherwise, <c>null</c> is returned.
    /// </summary>
    public Transform Target()
    {
        return GetMouseHitInfo().transform.GetComponent<Target>() != null
               ? GetMouseHitInfo().transform
               : null;
    }
    #endregion


    #region ─── Aim, Camera & Laser Updates ──────────────────────────────────────
    private void UpdateAimPosition()
    {
        Transform target = Target();

        // Lock directly onto a valid target when requested
        if (target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
                aim.position = target.GetComponent<Renderer>().bounds.center;
            else
                aim.position = target.position;

            return;
        }

        // Otherwise follow the mouse hit point
        aim.position = GetMouseHitInfo().point;

        // Unless in “precise-aim” mode, keep the helper at shoulder height
        if (!isAimingPrecise)
            aim.position = new Vector3(aim.position.x, transform.position.y + 0.95f, aim.position.z);
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(
            cameraTarget.position,
            DesiredCameraPosition(),
            cameraSensitivity * Time.deltaTime);
    }

    private void UpdateAimVisuals()
    {
        aimLaser.enabled = player.weapon.IsWeaponReady();

        if (!aimLaser.enabled)
            return;

        float tipLength = 0.5f;          // Extra segment to show “laser light fading” beyond the hit
        float gunDistance = player.weapon.CurrentWeapon().weaponMaximumDistance;            // Maximum laser range

        Vector3 laserDirection = player.weapon.BulletDirection();
        Vector3 endPoint = player.weapon.CurrentWeaponGunPoint().position + laserDirection * gunDistance;

        WeaponModel weaponModel = player.weaponVisuals.currentWeaponModel();

        weaponModel.gunPoint.LookAt(aim);
        weaponModel.transform.LookAt(aim);

        // Shorten the beam if we hit something
        if (Physics.Raycast(player.weapon.CurrentWeaponGunPoint().position, laserDirection, out RaycastHit hitInfo, gunDistance, aimLayerMask))
        {
            endPoint = hitInfo.point;
            tipLength = 0f; // Temporary
        }

        // Set the three points used by the LineRenderer
        aimLaser.SetPosition(0, player.weapon.CurrentWeaponGunPoint().position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + (laserDirection * tipLength));
    }
    #endregion


    #region ─── Utility Methods ──────────────────────────────────────────────────
    /// <summary>
    /// Calculates a camera position that stays between <c>minCameraDistance</c> and
    /// <c>maxCameraDistance</c> from the player, clamped along the direction of the aim.
    /// </summary>
    private Vector3 DesiredCameraPosition()
    {
        Vector3 desired = GetMouseHitInfo().point;
        Vector3 aimDir = (desired - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, desired);
        float clampedDist = Mathf.Clamp(distance, minCameraDistance, maxCameraDistance);

        desired = transform.position + aimDir * clampedDist;
        desired.y = transform.position.y + 1f; // Keep the camera roughly at head height

        return desired;
    }

    /// <summary>
    /// Raycasts from the main camera through the mouse position and returns the hit info.
    /// If nothing is hit this frame, the last known valid hit is returned instead.
    /// </summary>
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayerMask))
            lastKnownMouseHit = hitInfo;

        return lastKnownMouseHit;
    }
    #endregion
    

    #region ─── Input Bindings ───────────────────────────────────────────────────
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        // Cursor position
        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;

        // Precise-aim toggle (RMB)
        controls.Character.AimPrecisely.performed += context => isAimingPrecise = true;
        controls.Character.AimPrecisely.canceled += context => isAimingPrecise = false;

        // Lock-on toggle (C)
        controls.Character.LockIntoTarget.performed += context => isLockingToTarget = true;
        controls.Character.LockIntoTarget.canceled += context => isLockingToTarget = false;
    }
    #endregion
}
