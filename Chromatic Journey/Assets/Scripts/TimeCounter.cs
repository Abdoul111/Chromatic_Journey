using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter Instance { get; private set; }

    public Text timerText;
    public TextMeshProUGUI finalTimerText;

    private float timeElapsed = 0f;
    private bool isTiming = false;

    private Dictionary<string, float> levelStartTimes = new Dictionary<string, float>();
    private string lastSceneName = "";
    private float lastKnownElapsedTime = 0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist the timer across scenes
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (isTiming && HealthCounter.health != 0)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateFinalTimerText()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        if (finalTimerText != null)
        {
            finalTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    public void StartTimer()
    {
        isTiming = true;
    }

    public void SaveStartTime(string levelName)
    {
        levelStartTimes[levelName] = timeElapsed; // Save the current elapsed time for this level
        Debug.Log($"Start time for {levelName} saved: {timeElapsed}s");
    }

    public void LoadStartTime(string levelName)
    {
        if (levelStartTimes.ContainsKey(levelName))
        {
            timeElapsed = levelStartTimes[levelName];
            Debug.Log($"Start time for {levelName} restored: {timeElapsed}s");
        }
        else
        {
            Debug.Log($"No saved time for {levelName}, starting fresh.");
        }
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        UpdateTimerText();
    }

    private bool shouldResetTimerForLevel1 = false;

    public void SetResetTimerFlag(bool value)
    {
        shouldResetTimerForLevel1 = value;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Check if the scene is being reloaded
        bool isReloading = lastSceneName == sceneName;
        lastSceneName = sceneName;

        // Handle the final timer text in the Finish scene
        if (sceneName == "Finish")
        {
            StopTimer();
            levelStartTimes.Clear(); // Reset all saved times
            lastKnownElapsedTime = 0f; // Clear the last known elapsed time
            Debug.Log("Level start times reset.");

            GameObject finalTimeTextObject = GameObject.Find("FinalTimeText");
            if (finalTimeTextObject != null)
            {
                finalTimerText = finalTimeTextObject.GetComponent<TextMeshProUGUI>();
            }

            UpdateFinalTimerText();
            return; // No further logic for the "Finish" scene
        }

        // Link timerText in the new scene
        GameObject timeTextObject = GameObject.Find("TimeText");
        if (timeTextObject != null)
        {
            timerText = timeTextObject.GetComponent<Text>();
        }

        // Handle level-specific logic
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level 3 concept")
        {
            if (!isReloading)
            {
                if (sceneName == "Level1" && shouldResetTimerForLevel1)
                {
                    ResetTimer(); // Start fresh only if the flag is true
                    shouldResetTimerForLevel1 = false; // Reset the flag
                }
                else if (levelStartTimes.ContainsKey(sceneName))
                {
                    // Load the last known elapsed time for the level
                    timeElapsed = levelStartTimes[sceneName];
                }
                else
                {
                    // If no prior time exists, use the last known elapsed time
                    timeElapsed = lastKnownElapsedTime;
                }
            }

            StartTimer();
        }
    }

    // Called when transitioning to the main menu
    public void SaveCurrentTime()
    {
        lastKnownElapsedTime = timeElapsed; // Save the current elapsed time
        Debug.Log($"Saved current elapsed time: {lastKnownElapsedTime}s");
    }

}
