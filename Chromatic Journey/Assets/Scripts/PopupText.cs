using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupText : MonoBehaviour
{
    public static PopupText Instance;

    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;
    public Image popUpBoxImage; // Reference to the popup box's image component
    private bool isPopupActive = false;
    public Button popupButton;
    private SignManager currentSignManager;
    private Color originalTextColor;
    private Color originalBoxColor;
    private Color originalButtonColor;
    private Color originalButtonTextColor;


    private void Awake()
    {
        // Ensure only one instance of PopupManager exists.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }

        // Store original colors
        originalTextColor = popUpText.color;
        originalBoxColor = popUpBoxImage.color;
        originalButtonColor = popupButton.image.color;
        originalButtonTextColor = popupButton.GetComponentInChildren<TMP_Text>().color;
    }

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
        isPopupActive = true;

        // Re-enable the button for interaction
        popupButton.interactable = true;
    }

    public void RegisterSignManager(SignManager signManager)
    {
        // Stop audio from the previously registered sign
        if (currentSignManager != null && currentSignManager != signManager)
        {
            currentSignManager.StopAudio();
        }

        // Update the currentSignManager
        currentSignManager = signManager;
    }

    public void ClosePopUp()
    {
        if (isPopupActive)
        {
            Debug.Log("Button clicked!");
            animator.SetTrigger("close");
            popUpBox.SetActive(false);

            // Notify the current sign manager to reset its state
            if (currentSignManager != null)
            {
                currentSignManager.ResetPopup(); // Reset the state
                currentSignManager.StopAudio(); // Stop audio if playing
            }

            // Disable the button to prevent unintended clicks
            popupButton.interactable = false;

            // Clear button hover or focus state
            EventSystem.current.SetSelectedGameObject(null);

            isPopupActive = false;
        }
    }

    public void ResetState()
    {
        isPopupActive = false;
        currentSignManager = null; // Clear the reference to the current sign manager

        // Reactivate the popup box for future triggers
        popUpBox.SetActive(true);

        // Restore original colors
        popUpText.color = originalTextColor;
        popUpBoxImage.color = originalBoxColor;
        popupButton.image.color = originalButtonColor;
        popupButton.GetComponentInChildren<TMP_Text>().color = originalButtonTextColor;
    }

    public IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        // Reset colors to original values before starting fade
        popUpText.color = originalTextColor;
        popUpBoxImage.color = originalBoxColor;
        popupButton.image.color = originalButtonColor;
        popupButton.GetComponentInChildren<TMP_Text>().color = originalButtonTextColor;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

            // Gradually fade text, popup box, and button components
            popUpText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, alpha);
            popUpBoxImage.color = new Color(originalBoxColor.r, originalBoxColor.g, originalBoxColor.b, alpha);
            popupButton.image.color = new Color(originalButtonColor.r, originalButtonColor.g, originalButtonColor.b, alpha);
            popupButton.GetComponentInChildren<TMP_Text>().color = new Color(originalButtonTextColor.r, originalButtonTextColor.g, originalButtonTextColor.b, alpha);

            yield return null;
        }

        // Ensure everything is fully transparent after fade
        popUpText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0f);
        popUpBoxImage.color = new Color(originalBoxColor.r, originalBoxColor.g, originalBoxColor.b, 0f);
        popupButton.image.color = new Color(originalButtonColor.r, originalButtonColor.g, originalButtonColor.b, 0f);
        popupButton.GetComponentInChildren<TMP_Text>().color = new Color(originalButtonTextColor.r, originalButtonTextColor.g, originalButtonTextColor.b, 0f);

        // Hide the popup box
        popUpBox.SetActive(false);

        // Reset the state of PopupText after fade
        ResetState();
    }
}
