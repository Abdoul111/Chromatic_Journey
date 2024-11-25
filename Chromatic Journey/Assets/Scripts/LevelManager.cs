using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Loads Level 1
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1"); // Make sure the scene names match exactly
    }

    // Loads Level 2
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    // Loads Level 3
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3 concept");
    }
}
