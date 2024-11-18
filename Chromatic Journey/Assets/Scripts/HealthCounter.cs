using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class HealthCounter : MonoBehaviour
{
    public GameObject losePanel;
    public Text healthText;
    public static float health = 100;
    private AudioSource deathAudioSource;
    public AudioClip deathAudioSound;
    private AudioSource damageAudioSource;
    public AudioClip damageAudioSound;
    public int deathCount = 0;

    // Reference to the player's SpriteRenderer
    private SpriteRenderer playerSprite;

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
        AudioSource[] audioSourcesHealth = GetComponents<AudioSource>();
        if (audioSourcesHealth.Length >= 2)
        {
            deathAudioSource = audioSourcesHealth[0]; // First AudioSource for death
            damageAudioSource = audioSourcesHealth[1]; // Second AudioSource for damage
        }

        // Find player's SpriteRenderer
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
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
            if (Instance.deathCount == 0)
            {
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

        if (Instance.deathCount == 0)
        {
            if (Instance.damageAudioSource != null && !Instance.damageAudioSource.isPlaying)
            {
                Instance.damageAudioSource.clip = Instance.damageAudioSound; // Assign the damage sound
                Instance.damageAudioSource.Play(); // Play the sound
            }
        }

        // Trigger the red hue effect
        if (Instance.playerSprite != null)
        {
            Instance.StartCoroutine(Instance.ChangeToRed());
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

    // Coroutine to change color to red and revert back
    private IEnumerator ChangeToRed()
    {
        if (playerSprite != null)
        {
            Color originalColor = playerSprite.color; // Save the original color
            playerSprite.color = Color.red; // Change to red
            if (Instance.deathCount == 0){
                yield return new WaitForSeconds(0.25f); // Wait for 0.5 seconds
                playerSprite.color = originalColor; // Revert to original color
            }
        }
    }
}
