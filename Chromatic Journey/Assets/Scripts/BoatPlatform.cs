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
    private Vector3 previousPosition;
    private bool isMovingForward = true;
    private bool isFlipping = false;
    private float flipTimer = 0f;
    private float initialScaleX;

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
        previousPosition = transform.position;
        initialScaleX = transform.localScale.x;
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

        previousPosition = transform.position;
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
        float normalizedTime = flipTimer / flipDuration;
        float smoothStepTime = normalizedTime * normalizedTime * (3f - 2f * normalizedTime);
        float newScaleX = Mathf.Lerp(initialScaleX, -initialScaleX, smoothStepTime);
        Vector3 newScale = transform.localScale;
        newScale.x = newScaleX;
        transform.localScale = newScale;

        if (flipTimer >= flipDuration)
        {
            isFlipping = false;
            isMovingForward = !isMovingForward;
            newScale.x = isMovingForward ? initialScaleX : -initialScaleX;
            transform.localScale = newScale;
        }

        previousPosition = transform.position;
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