using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    public float bobSpeed = 2f; // Speed of bobbing
    public float bobHeight = 0.5f; // Maximum height of bobbing

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
