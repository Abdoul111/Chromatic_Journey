using UnityEngine;

public class KeepInPlace : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float parallaxFactor = 0.015f; // Adjust for how much movement to allow

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Adjust the object's position slightly relative to the player's movement
            transform.position = initialPosition + (player.position * parallaxFactor);
        }
    }
}
