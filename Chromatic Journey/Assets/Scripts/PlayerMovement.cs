using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Animator animator;
    int isRunningHash;

    [SerializeField] Camera mainCamera;
    private Vector3 cameraOffset = new Vector3(0, 5, -10);

    private float horizontal;
    private float speed = 7f;
    private float jumpingPower = 15f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
            rb.velocity = new Vector2(rb.velocity.x*0.3f, jumpingPower);

        }

        Flip();

        // make camera follow player
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
        }


        bool moving = horizontal != 0;
        bool isRunning = animator.GetBool(isRunningHash);
        //bool shiftPressed = Input.GetKey(KeyCode.LeftShift);


        if (!isRunning && (moving))
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && (!moving))
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
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
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
