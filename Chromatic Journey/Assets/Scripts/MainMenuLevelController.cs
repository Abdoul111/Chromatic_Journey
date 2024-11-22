using UnityEngine;

public static class MainMenuLevelController
{
    private const string LevelKey = "CurrentLevel"; // Key for PlayerPrefs

    // Gets the current level, defaulting to 1 if not set
    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1);
    }

    // Updates the current level and saves it
    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(LevelKey, level);
        PlayerPrefs.Save();
        Debug.Log($"Level updated to {level}");
    }
}
