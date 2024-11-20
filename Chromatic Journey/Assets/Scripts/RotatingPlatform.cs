using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    public bool isClockwise = true;
    public float radius = 3f;
    public float speed = 3f;
    [Range(0, 360)] public float startingAngleDegrees = 0f; // New parameter for starting position
    public Transform centerPoint;

    [Header("Movement Control")]
    public bool isMovingOnceOnly = false;
    private bool movementEnded = false;
    private bool startMoving = false;

    private float currentAngle;
    private Vector3 previousPosition;
    private Vector2 smoothedVelocity;

    private void Start()
    {
        previousPosition = transform.position;
        smoothedVelocity = Vector2.zero;

        // If no center point is assigned, create one at the current position
        if (centerPoint == null)
        {
            GameObject center = new GameObject("Platform_Center");
            center.transform.position = transform.position;
            centerPoint = center.transform;
        }

        // Convert starting angle from degrees to radians and set as initial angle
        currentAngle = startingAngleDegrees * Mathf.Deg2Rad;

        // Set platform to its starting position on the circle
        Vector3 offset = new Vector3(
            Mathf.Cos(currentAngle) * radius,
            Mathf.Sin(currentAngle) * radius,
            0f
        );
        transform.position = centerPoint.position + offset;
    }

    private void FixedUpdate()
    {
        if (isMovingOnceOnly && movementEnded) return;
        if (isMovingOnceOnly && !startMoving) return;

        float rotationDirection = isClockwise ? -1f : 1f;
        currentAngle += rotationDirection * speed * Time.deltaTime;

        Vector3 offset = new Vector3(
            Mathf.Cos(currentAngle) * radius,
            Mathf.Sin(currentAngle) * radius,
            0f
        );

        transform.position = centerPoint.position + offset;

        Vector2 rawVelocity = ((Vector2)(transform.position - previousPosition)) / Time.fixedDeltaTime;
        smoothedVelocity = Vector2.Lerp(smoothedVelocity, rawVelocity, 0.2f);
        previousPosition = transform.position;

        if (isMovingOnceOnly && currentAngle >= startingAngleDegrees * Mathf.Deg2Rad + (Mathf.PI * 2f))
        {
            movementEnded = true;
        }
    }

    public void TriggerMovement()
    {
        if (!startMoving && !movementEnded)
        {
            Debug.Log("Platform rotation triggered.");
            startMoving = true;
        }
    }

    public Vector2 GetPlatformVelocity()
    {
        return smoothedVelocity;
    }

    private void OnDrawGizmos()
    {
        if (centerPoint != null)
        {
            // Draw the circular path
            Gizmos.color = Color.green;
            DrawCircle(centerPoint.position, radius, 32);

            // Draw center point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerPoint.position, 0.2f);

            // Draw platform's starting position in blue
            float startAngleRad = startingAngleDegrees * Mathf.Deg2Rad;
            Vector3 startPos = centerPoint.position + new Vector3(
                Mathf.Cos(startAngleRad) * radius,
                Mathf.Sin(startAngleRad) * radius,
                0f
            );
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(startPos, 0.15f);

            // Draw current platform position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);

            // Draw radius line to starting position
            Gizmos.color = Color.white;
            Gizmos.DrawLine(centerPoint.position, startPos);
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = (2f * Mathf.PI) / segments;
        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep;
            float angle2 = (i + 1) * angleStep;

            Vector3 point1 = new Vector3(
                center.x + radius * Mathf.Cos(angle1),
                center.y + radius * Mathf.Sin(angle1),
                center.z
            );

            Vector3 point2 = new Vector3(
                center.x + radius * Mathf.Cos(angle2),
                center.y + radius * Mathf.Sin(angle2),
                center.z
            );

            Gizmos.DrawLine(point1, point2);
        }
    }
}