using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    // Method to load the main menu scene
    public void LoadMainMenuScene()
    {
        if (TimeCounter.Instance != null)
        {
            TimeCounter.Instance.SaveCurrentTime();
        }

        SceneManager.LoadScene("Main Menu");
        if (SceneManager.GetActiveScene().name == "Finish")
        {
            MainMenuLevelController.SetCurrentLevel(1);
        }
    }

    public void LoadFinalScene()
    {
        SceneManager.LoadScene("Finish");
    }
}
