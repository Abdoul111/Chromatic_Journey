using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer
    public VideoClip clip1;         // First video clip
    public VideoClip clip2;         // Second video clip
    public VideoClip clip3;         // Third video clip

    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned!");
            return;
        }

        int currentLevel = MainMenuLevelController.GetCurrentLevel();
        PlayVideoBasedOnCondition(currentLevel);
    }

    public void PlayVideoBasedOnCondition(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
                videoPlayer.clip = clip1;

                level1Button.interactable = true;
                level2Button.interactable = false;
                level3Button.interactable = false;



                Debug.Log("Playing clip 1");
                break;
            case 2:
                videoPlayer.clip = clip2;

                level1Button.interactable = true;
                level2Button.interactable = true;
                level3Button.interactable = false;

                Debug.Log("Playing clip 2");
                break;
            case 3:
                videoPlayer.clip = clip3;

                level1Button.interactable = true;
                level2Button.interactable = true;
                level3Button.interactable = true;

                Debug.Log("Playing clip 3");
                break;
            default:
                Debug.LogError("Invalid condition!");
                return;
        }

        // Play the selected clip
        videoPlayer.Play();
    }
}
