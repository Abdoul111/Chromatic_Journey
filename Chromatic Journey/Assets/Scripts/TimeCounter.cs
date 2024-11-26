using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public Text timerText;
    public TextMeshProUGUI finalTimerText;
    private float timeElapsed = 0f;
    private bool isTiming = false;
    private static TimeCounter instance;

    private void Awake()
    {
        // Singleton to ensure only one timer persists across scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        instance = this;
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
        Debug.Log("StopTimer");
        isTiming = false;
    }

    public void StartTimer()
    {
        Debug.Log("StartTimer");
        isTiming = true;
    }

    public float GetTimeElapsed()
    {
        Debug.Log(timeElapsed);
        return timeElapsed;
    }

    public void ResetTimer()
    {
        Debug.Log("ResetTimer");
        timeElapsed = 0f;
        UpdateTimerText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure timerText is re-linked if the UI is reloaded
        if (timerText == null)
        {
            GameObject timeTextObject = GameObject.Find("TimeText");
            if (timeTextObject != null)
            {
                timerText = timeTextObject.GetComponent<Text>();
            }
        }

        // Handle the final timer text in the Finish scene
        if (scene.name == "Finish")
        {
            StopTimer();

            // Find the finalTimerText in the new scene
            GameObject finalTimeTextObject = GameObject.Find("FinalTimeText");
            if (finalTimeTextObject != null)
            {
                finalTimerText = finalTimeTextObject.GetComponent<TextMeshProUGUI>();
            }

            // Update the final timer text
            UpdateFinalTimerText();
            MainMenuLevelController.SetCurrentLevel(1);
        }

        // Reset timer if the player goes to the Main Menu
        if (scene.name == "Main Menu")
        {
            StopTimer();
        }

        // Start the timer when entering Level1
        if (scene.name == "Level1")
        {
            ResetTimer();
            StartTimer();
        }

        if (scene.name == "Level2")
        {
            StartTimer();
        }

        if (scene.name == "Level 3 concept")
        {
            StartTimer();
        }
    }
}
