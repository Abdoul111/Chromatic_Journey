using UnityEngine;
using TMPro;

public class ButtonTrigger : MonoBehaviour
{
    [Header("Core References")]
    public MovingLever leverToTrigger;

    [Header("Button Settings")]
    [Range(1, 6)]
    public int buttonSequenceNumber = 1;
    public float pressedDistance = 0.5f;
    public bool moveHorizontally = true;

    [Header("Text Display Settings")]
    public TMP_FontAsset customFont;
    public Color textColor = Color.white;
    public float textYOffset = 1f;
    public float fontSize = 3f;

    private bool hasBeenPressed = false;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private TextMeshPro buttonText;
    private static int currentButtonInSequence = 1; // Shared between all buttons

    private void Start()
    {
        originalPosition = transform.position;
        SetupButtonText();

        // Calculate pressed position based on direction
        if (moveHorizontally)
        {
            pressedPosition = originalPosition + new Vector3(pressedDistance, 0, 0);
        }
        else
        {
            pressedPosition = originalPosition + new Vector3(0, pressedDistance, 0);
        }
    }

    private void SetupButtonText()
    {
        // Create a new GameObject for the text
        GameObject textObj = new GameObject($"Button{buttonSequenceNumber}Text");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, textYOffset, 0);

        // Add TextMeshPro component
        buttonText = textObj.AddComponent<TextMeshPro>();
        buttonText.text = buttonSequenceNumber.ToString();
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.fontSize = fontSize;

        if (customFont != null)
        {
            buttonText.font = customFont;
        }

        buttonText.color = textColor;
        buttonText.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        // Keep text facing camera
        if (buttonText != null && Camera.main != null)
        {
            buttonText.transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenPressed && collision.CompareTag("Player"))
        {
            // Only respond if it's this button's turn in the sequence
            if (buttonSequenceNumber == currentButtonInSequence)
            {
                PressButton();
                leverToTrigger.TriggerLever();
                currentButtonInSequence++; // Move to next button in sequence
                Debug.Log($"Button {buttonSequenceNumber} pressed. Next button in sequence: {currentButtonInSequence}");
            }
            else
            {
                Debug.Log($"Button {buttonSequenceNumber} cannot be pressed yet. Current sequence number: {currentButtonInSequence}");
            }
        }
    }

    private void PressButton()
    {
        hasBeenPressed = true;
        transform.position = pressedPosition;

        // Update text position
        if (buttonText != null)
        {
            buttonText.transform.position = transform.position + new Vector3(0, textYOffset, 0);
        }
    }

    private void OnValidate()
    {
        // Update text properties in editor
        if (buttonText != null)
        {
            buttonText.color = textColor;
            buttonText.fontSize = fontSize;
            if (customFont != null)
            {
                buttonText.font = customFont;
            }
            buttonText.transform.localPosition = new Vector3(0, textYOffset, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;

        Vector3 start = transform.position;
        Vector3 end;

        if (moveHorizontally)
        {
            end = start + new Vector3(pressedDistance, 0, 0);
        }
        else
        {
            end = start + new Vector3(0, pressedDistance, 0);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(end, 0.1f);
    }
}