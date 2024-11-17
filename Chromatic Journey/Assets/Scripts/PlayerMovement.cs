using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isJumpingHash;

    [SerializeField] Camera mainCamera;
    private Vector3 cameraOffset = new Vector3(0, 5, -10);

    private float horizontal;
    private float speed = 7f;
    private float jumpingPower = 15f;
    private bool isFacingRight = true;
    private MovingPlatform currentPlatform;
    private Vector2 platformVelocity;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer; // Assign ground layer in the inspector
    [SerializeField] private Transform groundCheck; // Assign a ground-check transform
    private float groundCheckRadius = 0.1f;
    private bool isGrounded;
    private bool isInAir = false;
    private float jumpCooldown = 0.2f; // Minimum delay before checking ground
    private float jumpTimer = 0f; // Timer to track cooldown
    private float maxAirTime = 1.0f; // Maximum time allowed in the air before stopping animation
    private float airTime = 0f; // Timer to track how long the player is in the air

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        cameraOffset = mainCamera.transform.position - transform.position;
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        bool jumping = Input.GetKeyDown(KeyCode.Space);

        // Decrease the jump timer
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }

        // Check if grounded (only if not in cooldown)
        if (jumpTimer <= 0)
        {
            // Adjust ground check to account for platform movement
            if (currentPlatform != null)
            {
                Vector2 platformVelocity = currentPlatform.GetPlatformVelocity();
                isGrounded = Physics2D.OverlapCircle(groundCheck.position + (Vector3)platformVelocity * Time.deltaTime, groundCheckRadius, groundLayer);
            }
            else
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            }
        }

        // Track air time
        if (!isGrounded)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime = 0f; // Reset air time when grounded
        }

        // Jump logic
        if (jumping && isGrounded && jumpTimer <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool(isJumpingHash, true); // Start jump animation
            isInAir = true; // Mark the player as in the air
            airTime = 0f; // Reset air time
            jumpTimer = jumpCooldown; // Start cooldown
        }

        // Stop jump animation if air time exceeds maxAirTime
        if (isInAir && airTime > maxAirTime)
        {
            animator.SetBool(isJumpingHash, false); // Stop jump animation
            isInAir = false; // Reset air status
        }

        // Reset jump animation only when landing after being in the air
        if (isInAir && isGrounded && jumpTimer <= 0)
        {
            animator.SetBool(isJumpingHash, false); // Stop jump animation
            isInAir = false; // Reset air status
        }

        Flip();

        // Make camera follow player
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
        }

        // Handle running animation
        bool moving = horizontal > 0.1f || horizontal < -0.1f;
        bool isRunning = animator.GetBool(isRunningHash);

        if (!isRunning && moving)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (isRunning && !moving)
        {
            animator.SetBool(isRunningHash, false);
        }

        AlignToGround();
    }
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;

        // Move the player based on input only
        velocity.x = horizontal * speed;

        // Preserve vertical velocity for gravity/jumping
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Attach player to platform when standing on it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // Attach the player to the platform
            transform.parent = collision.transform;
            currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // Detach the player from the platform
            transform.parent = null;
            currentPlatform = null;
        }
    }


    void AlignToGround()
    {
        // Cast a ray downward to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, groundLayer);

        if (hit.collider != null)
        {
            Vector2 groundNormal = hit.normal;
            float angle = Mathf.Atan2(groundNormal.y, groundNormal.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

}