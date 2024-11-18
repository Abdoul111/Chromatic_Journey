using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 3f;

    private Vector3 direction;
    private Vector3 previousPosition;
    private Vector2 smoothedVelocity; // To smooth out erratic velocity changes
    private bool isMovingForward = true; // Track movement direction
    public bool isMovingOnceOnly = false;
    private bool movementEnded = false;
    private bool startMoving = false;

    private void Start()
    {
        direction = (endPoint.position - startPoint.position).normalized;
        previousPosition = transform.position;
        smoothedVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        // true when the player solves the puzzle
        if (isMovingOnceOnly && movementEnded) return;

        // true when the platform is still not moving
        if (isMovingOnceOnly && !startMoving) return;

        // Move the platform
        transform.position += direction * speed * Time.deltaTime;

        // Reverse direction at endpoints
        if (Vector3.Distance(transform.position, endPoint.position) < 0.1f && isMovingForward)
        {
            movementEnded = true;
            direction = (startPoint.position - endPoint.position).normalized;
            isMovingForward = false;
        }
        else if (Vector3.Distance(transform.position, startPoint.position) < 0.1f && !isMovingForward)
        {
            direction = (endPoint.position - startPoint.position).normalized;
            isMovingForward = true;
        }

        // Calculate velocity and smooth it out
        Vector2 rawVelocity = ((Vector2)(transform.position - previousPosition)) / Time.fixedDeltaTime;
        smoothedVelocity = Vector2.Lerp(smoothedVelocity, rawVelocity, 0.2f); // Smooth changes
        previousPosition = transform.position;
    }

    public void TriggerMovement()
    {
        if (!startMoving && !movementEnded)
        {
            Debug.Log("Platform movement triggered.");
            startMoving = true;
        }
    }

    public Vector2 GetPlatformVelocity()
    {
        return smoothedVelocity;
    }

    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            // Draw the trajectory line
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);

            // Draw spheres at the start and end points
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPoint.position, 0.1f); // Small sphere at the start point
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(endPoint.position, 0.1f); // Small sphere at the end point
        }

        // Draw the platform's current position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f); // Current position of the platform
    }
}
