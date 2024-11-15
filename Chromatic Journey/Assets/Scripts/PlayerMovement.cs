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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer; // Assign ground layer in the inspector
    [SerializeField] private Transform groundCheck; // Assign a ground-check transform
    private float groundCheckRadius = 0.01f;
    private bool isGrounded;
    private bool isInAir = false;
    private float jumpCooldown = 0.2f; // Minimum delay before checking ground
    private float jumpTimer = 0f; // Timer to track cooldown
    private float maxAirTime = 0.6f; // Maximum time allowed in the air before stopping animation
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

    void Update()
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
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
            Debug.Log("Jump animation triggered.");
        }

        // Stop jump animation if air time exceeds maxAirTime
        if (isInAir && airTime > maxAirTime)
        {
            animator.SetBool(isJumpingHash, false); // Stop jump animation
            isInAir = false; // Reset air status
            Debug.Log("Player exceeded max air time, stopping jump animation.");
        }

        // Reset jump animation only when landing after being in the air
        if (isInAir && isGrounded && jumpTimer <= 0)
        {
            animator.SetBool(isJumpingHash, false); // Stop jump animation
            isInAir = false; // Reset air status
            Debug.Log("Player landed, resetting jump animation.");
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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
            transform.parent = collision.transform;
        }
    }

    // Detach player from platform when they leave it
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }
}