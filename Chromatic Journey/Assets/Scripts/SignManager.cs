using System.Collections;
using UnityEngine;

public class SignManager : MonoBehaviour
{
    [SerializeField] public string popUpString;
    [SerializeField] private AudioClip audioPopup1;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop and reset any active popup and audio
            PopupText.Instance.StopAndResetCurrentPopup();

            // Set this sign as the new active sign
            PopupText.Instance.RegisterSignManager(this);

            // Play the audio and show the popup for this sign
            DisplayPopupAndAudio();
        }
    }

    private void DisplayPopupAndAudio()
    {
        // Play audio
        if (audioPopup1 != null)
        {
            audioSource.Stop(); // Stop any ongoing audio
            audioSource.PlayOneShot(audioPopup1);
        }

        // Show popup text
        PopupText.Instance.PopUp(popUpString);

        // Trigger fade-out after the audio ends
        if (audioPopup1 != null)
        {
            PopupText.Instance.StartFadeOut(audioPopup1.length, 1f); // Fade after audio
        }
    }

    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ResetPopupState()
    {
        StopAllCoroutines(); // Stop any running coroutines for this sign
    }
}
