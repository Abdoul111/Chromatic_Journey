using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel1()
    {
        // Set the flag to reset the timer when Level 1 is loaded
        if (TimeCounter.Instance != null)
        {
            TimeCounter.Instance.SetResetTimerFlag(true);
        }
        
        SceneManager.LoadScene("Level1");
        MainMenuLevelController.SetCurrentLevel(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
        MainMenuLevelController.SetCurrentLevel(2);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3 concept");
        MainMenuLevelController.SetCurrentLevel(3);
    }
}
