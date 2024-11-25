using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
        isPopupActive = true;
    }

    public void RegisterSignManager(SignManager signManager)
    {
        currentSignManager = signManager;
    }

    public void ClosePopUp()
   {
        if (isPopupActive)
        {
            Debug.Log("Button clicked!");
            animator.SetTrigger("close");
            popUpBox.SetActive(false);
            currentSignManager?.StopAudio();
        }
   }

    public IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        // Save original colors
        Color textOriginalColor = popUpText.color;
        Color boxOriginalColor = popUpBoxImage.color;
        Color buttonImageOriginalColor = popupButton.image.color; // Button background
        Color buttonTextOriginalColor = popupButton.GetComponentInChildren<TMP_Text>().color; // Button text

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

            // Fade text, popup box, and button components
            popUpText.color = new Color(textOriginalColor.r, textOriginalColor.g, textOriginalColor.b, alpha);
            popUpBoxImage.color = new Color(boxOriginalColor.r, boxOriginalColor.g, boxOriginalColor.b, alpha);
            popupButton.image.color = new Color(buttonImageOriginalColor.r, buttonImageOriginalColor.g, buttonImageOriginalColor.b, alpha);
            popupButton.GetComponentInChildren<TMP_Text>().color = new Color(buttonTextOriginalColor.r, buttonTextOriginalColor.g, buttonTextOriginalColor.b, alpha);

            yield return null;
        }

        // Ensure everything is fully transparent
        popUpText.color = new Color(textOriginalColor.r, textOriginalColor.g, textOriginalColor.b, 0f);
        popUpBoxImage.color = new Color(boxOriginalColor.r, boxOriginalColor.g, boxOriginalColor.b, 0f);
        popupButton.image.color = new Color(buttonImageOriginalColor.r, buttonImageOriginalColor.g, buttonImageOriginalColor.b, 0f);
        popupButton.GetComponentInChildren<TMP_Text>().color = new Color(buttonTextOriginalColor.r, buttonTextOriginalColor.g, buttonTextOriginalColor.b, 0f);

        popUpBox.SetActive(false);
    }
}
