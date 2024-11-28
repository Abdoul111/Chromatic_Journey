using UnityEngine;

public class MovingLever : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 3f;
    public GameObject triggerButton;

    [Header("Camera View")]
    public LeverCameraView cameraView; // Reference to the camera view component

    private Vector3 direction;
    private bool hasStartedMoving = false;
    private bool hasReachedEnd = false;

    private void Start()
    {
        transform.position = startPoint.position;
        direction = (endPoint.position - startPoint.position).normalized;
    }

    private void FixedUpdate()
    {
        if (hasStartedMoving && !hasReachedEnd)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                hasReachedEnd = true;
                transform.position = endPoint.position;
                Debug.Log("Lever has reached end position");
            }
        }
    }

    public void TriggerLever()
    {
        if (!hasStartedMoving)
        {
            hasStartedMoving = true;

            // Show the lever movement in the camera view
            if (cameraView != null)
            {
                cameraView.ShowLeverMovement(this);
            }

            Debug.Log("Lever movement triggered");
        }
    }

    // Rest of the code remains the same...
}