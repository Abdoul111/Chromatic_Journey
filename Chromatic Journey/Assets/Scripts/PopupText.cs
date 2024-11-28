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
    public Image popUpBoxImage;
    public Button popupButton;

    private SignManager currentSignManager;
    private bool isPopupActive = false;
    private Coroutine fadeCoroutine;

    private readonly Color semiTransparentGray = new Color(0.2f, 0.2f, 0.2f, 0.8f); // Semi-transparent gray
    private readonly Color fullyOpaque = new Color(1f, 1f, 1f, 1f); // Fully opaque color

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Set up button click functionality
        if (popupButton != null)
        {
            popupButton.onClick.AddListener(ClosePopUp);
        }
    }

    public void PopUp(string text)
    {
        // Stop any existing fade coroutine to prevent conflicts
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Set initial colors for transparency
        SetInitialTransparency();

        // Display the popup box and set its text
        popUpBox.SetActive(true);
        popUpText.text = text;

        // Trigger the animation
        animator.SetTrigger("pop");
        isPopupActive = true;

        // Enable the button for interaction
        popupButton.interactable = true;
    }

    public void RegisterSignManager(SignManager signManager)
    {
        currentSignManager = signManager;
    }

    public void StopAndResetCurrentPopup()
    {
        if (currentSignManager != null)
        {
            // Stop the audio of the currently active sign
            currentSignManager.StopAudio();

            // Reset the current sign's state
            currentSignManager.ResetPopupState();
        }

        // Stop popup animations and fade coroutines
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Immediately hide the popup box
        ResetPopupUI();
    }

    private void ResetPopupUI()
    {
        isPopupActive = false;

        // Stop the animator and reset the popup box
        animator.ResetTrigger("pop");
        animator.ResetTrigger("close");
        popUpBox.SetActive(false);

        // Clear the current sign reference
        currentSignManager = null;

        // Clear button hover and focus state
        ClearButtonState();
    }

    private void SetInitialTransparency()
    {
        // Set popup box background to semi-transparent gray
        popUpBoxImage.color = semiTransparentGray;

        // Set popup text and button to fully opaque
        popUpText.color = fullyOpaque;
        popupButton.image.color = fullyOpaque;

        // Ensure button text is fully visible
        popupButton.GetComponentInChildren<TMP_Text>().color = fullyOpaque;
    }

    public void ClosePopUp()
    {
        if (isPopupActive)
        {
            // Stop audio immediately
            if (currentSignManager != null)
            {
                currentSignManager.StopAudio();
            }

            // Trigger the fade-out coroutine
            StartFadeOut(0f, 1f);
        }
    }

    public void StartFadeOut(float delay, float fadeDuration)
    {
        // Stop any existing fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start a new fade-out coroutine
        fadeCoroutine = StartCoroutine(FadeOutAfterDelay(delay, fadeDuration));
    }

    private void ClearButtonState()
    {
        // Deselect the button in the EventSystem
        EventSystem.current.SetSelectedGameObject(null);

        // Re-enable the button after resetting its state
        popupButton.interactable = true;
    }

    public IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;

        // Gradually fade out the popup components
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

            // Fade out text, popup box, and button components
            popUpText.color = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, alpha);
            popUpBoxImage.color = new Color(semiTransparentGray.r, semiTransparentGray.g, semiTransparentGray.b, alpha);
            popupButton.image.color = new Color(fullyOpaque.r, fullyOpaque.g, fullyOpaque.b, alpha);
            popupButton.GetComponentInChildren<TMP_Text>().color = new Color(fullyOpaque.r, fullyOpaque.g, fullyOpaque.b, alpha);

            yield return null;
        }

        // Reset the popup UI after fading out
        ResetPopupUI();
    }
}
