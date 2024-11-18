using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class HealthCounter : MonoBehaviour
{
    public GameObject losePanel;
    public Text healthText;
    public static float health = 100;

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
        if (healthText == null)
        {
            healthText = GameObject.Find("HealthText").GetComponent<Text>();
        }
        if (losePanel != null)
        {
            losePanel.SetActive(false); 
        }
        health = 100;
    }

    public static void Damage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0); // Clamp health to prevent negative values
        if (Instance.healthText != null)
        {
            Instance.healthText.text = Math.Round(health).ToString();
        }

        if (health == 0)
        {
            Instance.ShowLosePanel(); // Use instance to access non-static methods
            FindObjectOfType<PlayerMovement>().KillPlayer(); // Stop player movement
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
