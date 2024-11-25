using UnityEngine;

public class KeepInPlace : MonoBehaviour
{
    public Transform sign; // Reference to the specific sign this particle system belongs to

    private Vector3 initialOffset; // Offset of the particle system relative to the sign

    void Start()
    {
        if (sign == null)
        {
            Debug.LogError("Sign reference is missing! Assign a sign in the inspector.");
            return;
        }

        // Calculate the initial offset of the particle system relative to the sign
        initialOffset = transform.position - sign.position;
    }

    void LateUpdate()
    {
        if (sign != null)
        {
            // Keep the particle system anchored to the sign
            transform.position = sign.position + initialOffset;
        }
    }
}
