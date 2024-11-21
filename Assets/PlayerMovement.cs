using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpHeight = 2f;
    public float deadZone = 0.2f; // Threshold to ignore small inputs

    private float jumpVelocity;
    private Rigidbody2D rb;
    private Vector2 movement;

    public LayerMask groundLayer;
    private Collider2D coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        jumpVelocity = Mathf.Sqrt(2 * gravity * maxJumpHeight);
    }

    void Update()
    {
        // Get inputs from keyboard and gamepad
        float moveInput = Input.GetAxisRaw("Horizontal"); // Keyboard
        float gamepadInput = Input.GetAxisRaw("GamepadHorizontal"); // Gamepad

        // Apply dead zone to gamepad input
        if (Mathf.Abs(gamepadInput) < deadZone)
        {
            gamepadInput = 0;
        }

        // Use the larger input magnitude (keyboard or gamepad)
        movement.x = Mathf.Abs(gamepadInput) > Mathf.Abs(moveInput) ? gamepadInput : moveInput;

        // Handle jumping
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("GamepadJump")) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    bool IsGrounded()
    {
        // Check if the player is touching the ground
        return Physics2D.IsTouchingLayers(coll, groundLayer);
    }
}
