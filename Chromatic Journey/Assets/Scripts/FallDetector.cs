using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FallDetector : MonoBehaviour
{
    public GameObject losePanel; 

    private void Start()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(false); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowLosePanel();
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
