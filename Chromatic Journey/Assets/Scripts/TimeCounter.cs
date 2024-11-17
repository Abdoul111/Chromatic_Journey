using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCounter : MonoBehaviour
{
    public Text timerText;
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
        if (isTiming)
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

    public void StopTimer()
    {
        isTiming = false;
    }

    public void StartTimer()
    {
        isTiming = true;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public void ResetTimer()
    {
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

        // When in FinalScene, the timer stops to get points
        if (scene.name == "FinalScene")
        {
            StopTimer();
            Debug.Log("Final time: " + GetTimeElapsed() + " seconds");
        }

        // Reset timer if the player goes to the Main Menu
        if (scene.name == "Main Menu")
        {
            ResetTimer();
        }

        // Start the timer when entering Level1
        if (scene.name == "Level1")
        {
            StartTimer();
        }
    }
}
