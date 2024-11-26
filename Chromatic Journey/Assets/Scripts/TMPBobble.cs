using UnityEngine;
using TMPro;

public class TMPBobble : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
    public float bobbleSpeed = 2f; // Speed of the bobbing motion
    public float bobbleHeight = 10f; // Height of the bobbing motion

    private Vector3 originalPosition;

    private void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Store the original position of the text
        originalPosition = text.rectTransform.localPosition;
    }

    private void Update()
    {
        if (text != null)
        {
            // Calculate the new Y position using a sine wave
            float newY = originalPosition.y + Mathf.Sin(Time.time * bobbleSpeed) * bobbleHeight;

            // Apply the new position to the text
            text.rectTransform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }
}
