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
    [SerializeField] private float footstepCooldown = 0.5f; // Time between footsteps
    private AudioSource footstepAudioSource;
    public AudioClip footstepAudioSound;
    private AudioSource landingAudioSource;
    public AudioClip landingAudioSound;
    private AudioSource jumpAudioSource;
    public AudioClip jumpAudioSound;
    private float groundCheckRadius = 0.5f;
    private bool isGrounded;
    private bool isInAir = false;
    private float jumpCooldown = 0.2f; // Minimum delay before checking ground
    private float jumpTimer = 0f; // Timer to track cooldown
    private float maxAirTime = 1.0f; // Maximum time allowed in the air before stopping animation
    private float airTime = 0f; // Timer to track how long the player is in the air
    public bool isBusy = false; // to stop character interaction when it is making input sequences to solve puzzles
    // Flag to determine if the player is alive
    private bool isAlive = true;
    private float footstepTimer = 0f; // Timer to track cooldown

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
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 3)
        {
            footstepAudioSource = audioSources[1]; // First AudioSource for footsteps
            landingAudioSource = audioSources[2]; // Second AudioSource for landing
            jumpAudioSource = audioSources[3]; // Third AudioSource for jumping
        }
    }

    private void Update()
    {
        if (!isAlive || isBusy)
        {
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isRunningHash, false);
            isInAir = false;



            return; // Stop all input handling if the player is not alive
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        bool jumping = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        // Footstep logic: Play sound only if grounded and moving with input
        if (isGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstepAudio();
                footstepTimer = footstepCooldown; // Reset timer
            }
        }
        else
        {
            if (footstepAudioSource != null && footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
            footstepTimer = 0f; // Reset timer
        }

        // Decrease the jump timer
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }

        // Check if grounded (only if not in cooldown)
        if (jumpTimer <= 0)
        {
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
            airTime = 0f;
        }

        // Jump logic
        if (jumping && isGrounded && jumpTimer <= 0)
        {
            PlayJumpAudio();
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool(isJumpingHash, true);
            isInAir = true;
            airTime = 0f;
            jumpTimer = jumpCooldown;
        }

        // Stop jump animation if air time exceeds maxAirTime
        if (isInAir && airTime > maxAirTime)
        {
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isRunningHash, false);
            isInAir = false;
        }

        // Reset jump animation only when landing after being in the air
        if (isInAir && isGrounded && jumpTimer <= 0)
        {
            animator.SetBool(isJumpingHash, false);
            isInAir = false;
            PlayLandingAudio();
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

    void AlignToGround()
    {
        if (isGrounded) // Only align to ground when grounded
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, groundLayer);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && (!hit.collider.CompareTag("pushable")))
            {

                Vector2 groundNormal = hit.normal;
                float angle = Mathf.Atan2(groundNormal.y, groundNormal.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
        else
        {
            // Reset rotation to default (standing upright) when in the air
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void PlayFootstepAudio()
    {
        if (footstepAudioSource != null && !footstepAudioSource.isPlaying)
        {
            footstepAudioSource.clip = footstepAudioSound; // Assign the footstep sound
            footstepAudioSource.Play(); // Play the sound
        }
    }

    private void PlayLandingAudio()
    {
        if (landingAudioSource != null)
        {
            landingAudioSource.Stop();
            landingAudioSource.clip = landingAudioSound; // Assign the landing sound
            landingAudioSource.Play(); // Play the sound
        }
    }

    private void PlayJumpAudio()
    {
        if (jumpAudioSource != null)
        {
            jumpAudioSource.Stop();
            jumpAudioSource.clip = jumpAudioSound; // Assign the landing sound
            jumpAudioSource.Play(); // Play the sound
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive || isBusy)
        {
            rb.velocity = Vector2.zero;
            return; // Stop physics updates if the player is not alive
        }
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

    public void KillPlayer()
    {
        isAlive = false;
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetBool(isRunningHash, false); // Stop running animation
        animator.SetBool(isJumpingHash, false); // Stop jumping animation
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") || collision.gameObject.CompareTag("MovingBoat"))
        {
            transform.parent = collision.transform;
            currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") || collision.gameObject.CompareTag("MovingBoat"))
        {
            transform.parent = null;
            currentPlatform = null;
        }
    }
}
