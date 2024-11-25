using UnityEngine;

public class MovingLever : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 3f;
    public GameObject triggerButton; // Reference to the button that triggers the lever

    private Vector3 direction;
    private bool hasStartedMoving = false;
    private bool hasReachedEnd = false;

    private void Start()
    {
        // Set initial position to start point
        transform.position = startPoint.position;
        // Calculate direction once since it won't change
        direction = (endPoint.position - startPoint.position).normalized;
    }

    private void FixedUpdate()
    {
        // Only move if triggered and hasn't reached the end
        if (hasStartedMoving && !hasReachedEnd)
        {
            // Move the lever
            transform.position += direction * speed * Time.deltaTime;

            // Check if we've reached the end point
            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                hasReachedEnd = true;
                transform.position = endPoint.position; // Snap to exact position
                Debug.Log("Lever has reached end position");
            }
        }
    }

    // Call this method when the button is pressed
    public void TriggerLever()
    {
        if (!hasStartedMoving)
        {
            hasStartedMoving = true;
            Debug.Log("Lever movement triggered");
        }
    }

    private void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            // Draw the path line
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, endPoint.position);

            // Draw spheres at the start and end points
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startPoint.position, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(endPoint.position, 0.1f);
        }

        // Draw the lever's current position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}