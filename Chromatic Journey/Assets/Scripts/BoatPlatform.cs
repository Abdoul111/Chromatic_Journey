using UnityEngine;

public class BoatPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 3f;

    [Header("Endpoint Detection")]
    [Tooltip("How close the platform needs to be to endpoints to trigger flip")]
    public float endpointDetectionRadius = 0.5f;

    [Header("Flip Animation")]
    [Range(0.1f, 2f)]
    public float flipDuration = 0.5f;

    [Header("Movement Control")]
    public bool isMovingOnceOnly = false;
    private bool movementEnded = false;
    private bool startMoving = false;

    private Vector3 direction;
    private bool isMovingForward = true;
    private bool isFlipping = false;
    private float flipTimer = 0f;

    public Transform boat;

    // Add these for visual debugging of detection radius
    private void OnDrawGizmosSelected()
    {
        if (startPoint != null && endPoint != null)
        {
            // Draw detection spheres at endpoints
            Gizmos.color = new Color(1, 0, 0, 0.3f); // Semi-transparent red
            Gizmos.DrawWireSphere(startPoint.position, endpointDetectionRadius);
            Gizmos.DrawWireSphere(endPoint.position, endpointDetectionRadius);
        }
    }

    private void Start()
    {
        direction = (endPoint.position - startPoint.position).normalized;
    }

    private void FixedUpdate()
    {
        if (isMovingOnceOnly && movementEnded) return;
        if (isMovingOnceOnly && !startMoving) return;

        if (isFlipping)
        {
            HandleFlip();
            return;
        }

        // Move the boat
        transform.position += direction * speed * Time.deltaTime;

        // Check if we reached endpoints with larger detection radius
        if (Vector3.Distance(transform.position, endPoint.position) < endpointDetectionRadius && isMovingForward)
        {
            // Snap to exact position to prevent overshooting
            transform.position = endPoint.position;
            StartFlip();
        }
        else if (Vector3.Distance(transform.position, startPoint.position) < endpointDetectionRadius && !isMovingForward)
        {
            // Snap to exact position to prevent overshooting
            transform.position = startPoint.position;
            if (isMovingOnceOnly)
            {
                movementEnded = true;
                return;
            }
            StartFlip();
        }
    }

    // Rest of your original code remains the same...
    private void StartFlip()
    {
        isFlipping = true;
        flipTimer = 0f;
        if (isMovingForward)
        {
            direction = (startPoint.position - endPoint.position).normalized;
        }
        else
        {
            direction = (endPoint.position - startPoint.position).normalized;
        }
    }

    private void HandleFlip()
    {
        flipTimer += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(flipTimer / flipDuration);
        float rotationY = Mathf.LerpAngle(0f, 180f, normalizedTime); // Rotate only on Y-axis

        boat.eulerAngles = new Vector3(0, rotationY, -4);

        if (normalizedTime >= 1f)
        {
            isFlipping = false;
            isMovingForward = !isMovingForward;
            boat.eulerAngles = new Vector3(0, isMovingForward ? 0 : 180, -4);
        }
    }

    public void TriggerMovement()
    {
        if (!startMoving && !movementEnded)
        {
            Debug.Log("Boat movement triggered.");
            startMoving = true;
        }
    }
}