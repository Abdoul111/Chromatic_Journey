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

    [Header("Audio Settings")]
    [Tooltip("The audio clip to play for the boat's engine")]
    public AudioClip boatEngineSound;
    [Tooltip("Maximum distance at which the boat can be heard")]
    [Range(1f, 100f)]
    public float audioRadius = 20f;
    [Tooltip("How quickly the sound fades with distance")]
    [Range(0.1f, 5f)]
    public float audioRolloff = 1f;
    [Range(0f, 1f)]
    public float engineVolume = 1f;

    private Vector3 direction;
    private bool isMovingForward = true;
    private bool isFlipping = false;
    private float flipTimer = 0f;
    private AudioSource audioSource;

    public Transform boat;

    private void OnDrawGizmosSelected()
    {
        if (startPoint != null && endPoint != null)
        {
            // Draw detection spheres at endpoints
            Gizmos.color = new Color(1, 0, 0, 0.3f); // Semi-transparent red
            Gizmos.DrawWireSphere(startPoint.position, endpointDetectionRadius);
            Gizmos.DrawWireSphere(endPoint.position, endpointDetectionRadius);

            // Draw audio radius
            Gizmos.color = new Color(0, 1, 0, 0.1f); // Semi-transparent green
            Gizmos.DrawWireSphere(transform.position, audioRadius);
        }
    }

    private void Start()
    {
        direction = (endPoint.position - startPoint.position).normalized;
        SetupAudioSource();
    }

    private void SetupAudioSource()
    {
        // Add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource for 3D spatial audio
        audioSource.clip = boatEngineSound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f; // Full 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = audioRadius;
        audioSource.volume = engineVolume;
        audioSource.dopplerLevel = 0.5f; // Reduce Doppler effect
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.SetCustomCurve(
            AudioSourceCurveType.CustomRolloff,
            AnimationCurve.EaseInOut(0f, 1f, audioRadius, 0f)
        );
    }

    private void FixedUpdate()
    {
        if (isMovingOnceOnly && movementEnded)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        if (isMovingOnceOnly && !startMoving)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        // Start playing sound if not already playing
        if (!audioSource.isPlaying && boatEngineSound != null)
        {
            audioSource.Play();
        }

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
            transform.position = endPoint.position;
            StartFlip();
            return; // Add this line to stop movement
        }
        else if (Vector3.Distance(transform.position, startPoint.position) < endpointDetectionRadius && !isMovingForward)
        {
            transform.position = startPoint.position;
            if (isMovingOnceOnly)
            {
                movementEnded = true;
                audioSource.Stop();
                return;
            }
            StartFlip();
            return; // Add this line to stop movement
        }
    }

    // Existing methods remain the same
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
        float rotationY = Mathf.LerpAngle(0f, 180f, normalizedTime);

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