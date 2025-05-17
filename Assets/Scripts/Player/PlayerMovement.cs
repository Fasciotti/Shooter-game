using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* =====================
     *   Inspector Fields
     * ===================== */

    [Header("Movement Info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;

    /* =====================
     *   Private State
     * ===================== */

    private PlayerControls controls;
    private Player player;
    private CharacterController characterController;

    private float speed;            // Current move speed
    private float verticalVelocity; // Gravity accumulator
    

    public Vector2 moveInput {  get; private set; }
    private Vector3 movementDirection;
    private bool runButtonHeld = false;

    /* =====================
     *   Unity Messages
     * ===================== */

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();

        speed = walkSpeed;
        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        ApplyGravity();

        AnimatorControllers();
    }

    /* =====================
     *   Movement & Gravity
     * ===================== */

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(movementDirection * Time.deltaTime * speed);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
            movementDirection.y = verticalVelocity;
        }
    }

    /* =====================
     *   Aiming
     * ===================== */

    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    /* =====================
     *   Animation
     * ===================== */

    private void AnimatorControllers()
    {
        Vector3 animationMovementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);
        Animator playerAnimator = player.weaponVisuals.anim;

        float zVelocity = Vector3.Dot(animationMovementDirection.normalized, transform.forward);
        float xVelocity = Vector3.Dot(animationMovementDirection.normalized, transform.right);
        bool isRunningActive = runButtonHeld && animationMovementDirection.magnitude > 0;

        playerAnimator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);
        playerAnimator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        playerAnimator.SetBool("isRunning", isRunningActive);
    }

    /* =====================
     *   Input Handling
     * ===================== */

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Character.Run.performed += context =>
        {
            speed = runSpeed;
            runButtonHeld = true;
        };
        controls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            runButtonHeld = false;
        };
    }
}
