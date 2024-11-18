using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class HealthCounter : MonoBehaviour
{
    public GameObject losePanel;
    public Text healthText;
    public static float health = 100;
    private AudioSource deathAudioSource;
    public AudioClip deathAudioSound;
    public int deathCount = 0;

    // Singleton instance
    public static HealthCounter Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    void Start()
    {
        deathCount = 0;
        // Initialize UI components
        if (healthText == null)
        {
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
        }
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        // Reset health
        health = 100;

        // Assign AudioSource
        deathAudioSource = GetComponent<AudioSource>();
    }

    public static void Damage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0); // Clamp health to prevent negative values

        // Update health UI
        if (Instance.healthText != null)
        {
            Instance.healthText.text = Math.Round(health).ToString();
        }

        // Check if player is dead
        if (health == 0)
        {
            if (Instance.deathCount == 0){
                Instance.ShowLosePanel(); // Use instance to access non-static methods

                // Stop player movement
                PlayerMovement player = FindObjectOfType<PlayerMovement>();
                if (player != null)
                {
                    player.KillPlayer();
                }

                // Play death sound
                if (Instance.deathAudioSource != null && !Instance.deathAudioSource.isPlaying)
                {
                    Instance.deathAudioSource.clip = Instance.deathAudioSound; // Assign the death sound
                    Instance.deathAudioSource.Play(); // Play the sound
                }
                Instance.deathCount++;
            }
        }
    }

    private void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true); // Show the lose panel
        }
    }

    // Method to reload the scene, assigned to the "Try Again" button
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
