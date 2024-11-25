using System.Collections;
using UnityEngine;

public class SignManager : MonoBehaviour
{
    [SerializeField] public string popUpString;
    [SerializeField] private AudioClip audioPopup1;
    private AudioSource audioSource;
    private bool isPopupActive = false;

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
        if (other.CompareTag("Player") && !isPopupActive)
        {
            // Stop any ongoing coroutine in case of re-triggering
            StopAllCoroutines();

            isPopupActive = true;

            // Register this sign with PopupText
            PopupText.Instance.RegisterSignManager(this);

            // Reset popup box to ensure it's visible
            PopupText.Instance.popUpBox.SetActive(true);

            if (audioPopup1 != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioPopup1);
            }

            Debug.Log("Trigger detected with: " + other.gameObject.name);
            PopupText.Instance.PopUp(popUpString);

            // Start the popup handling coroutine
            StartCoroutine(HandlePopupAfterAudio());
        }
    }

    private IEnumerator HandlePopupAfterAudio()
    {
        // Wait for the audio to finish
        yield return new WaitForSeconds(audioPopup1.length);

        // Start fading out the popup text
        yield return PopupText.Instance.FadeOutAfterDelay(0f, 3f);

        // Allow the sign to be triggered again
        ResetPopup();
    }

    public void ResetPopup()
    {
        isPopupActive = false;
        StopAllCoroutines(); // Stop any ongoing coroutines
    }

    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            Debug.Log("Stopping audio for sign: " + gameObject.name);
            audioSource.Stop();
        }
    }
}
