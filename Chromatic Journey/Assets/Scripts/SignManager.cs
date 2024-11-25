using System.Collections;
using UnityEngine;

public class SignManager : MonoBehaviour
{
    [SerializeField] public string popUpString;
    [SerializeField] private AudioClip audioPopup1;
    private AudioSource audioSource;
    private bool hasInteracted = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PopupText.Instance.RegisterSignManager(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasInteracted)
        {
            hasInteracted = true;

            if (audioPopup1 != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioPopup1);
            }

            Debug.Log("Collision detected with: " + other.gameObject.name);
            PopupText.Instance.PopUp(popUpString);

            StartCoroutine(HandlePopupAfterAudio());
        }
    }

    private IEnumerator HandlePopupAfterAudio()
    {
        yield return new WaitForSeconds(audioPopup1.length);
        StartCoroutine(PopupText.Instance.FadeOutAfterDelay(0f, 3f));
    }

    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
