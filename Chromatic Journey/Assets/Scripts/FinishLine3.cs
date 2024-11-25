using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine3 : MonoBehaviour
{
    public coinHandler coinHandler; // Reference to the coinHandler script
    public AudioClip missingCoinsSound; // Sound to play when coins are missing
    private AudioSource audioSource; // AudioSource to play sounds
    private bool soundPlayed = false; // To track if the sound has been played

    private void Start()
    {
        // Add or get an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if all coins are collected
            if (coinHandler != null && coinHandler.AllCoinsCollected())
            {
                SceneManager.LoadScene("L3Success Cutscene");
            }
            else
            {
                Debug.Log("Not all coins collected!");
                PlayMissingCoinsSoundOnce();
            }
        }
    }

    private void PlayMissingCoinsSoundOnce()
    {
        if (missingCoinsSound != null && audioSource != null && !soundPlayed)
        {
            audioSource.PlayOneShot(missingCoinsSound);
            soundPlayed = true; // Prevent sound from playing again
            StartCoroutine(ResetSoundPlayed());
        }
    }

    private IEnumerator ResetSoundPlayed()
    {
        yield return new WaitForSeconds(8f); // Cooldown time (adjust as needed)
        soundPlayed = false; // Allow sound to play again after the cooldown
    }
}
