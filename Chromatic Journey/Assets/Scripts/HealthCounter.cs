using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour
{
    public GameObject losePanel;
    public static Text healthText;
    public static int health = 100;

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

    void Update()
    {
        if (health == 0)
        {
            ShowLosePanel();
        }
    }

    public static void Damage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);// Clamp health to prevent negative values
        healthText.text = health.ToString();

        // TODO: handle health drops to or below 0

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
