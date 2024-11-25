using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            //PopupText pop = GameObject.FindGameObjectWithTag("PopupManager").GetComponent<PopupText>();
            //pop.PopUp(popUpString);
            Debug.Log("Collision detected with: " + other.gameObject.name);
            PopupText.Instance.PopUp(popUpString);
        }
    }
}
