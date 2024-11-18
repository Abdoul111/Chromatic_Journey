using UnityEngine;

public class IntroMovementDelay : MonoBehaviour
{
    public MonoBehaviour targetScript; 
    public float delay = 3.0f;        

    void Start()
    {
        if (targetScript != null)
        {
            targetScript.enabled = false; // Ensure the script is initially disabled
            Invoke(nameof(EnableScript), delay); // Enable the script after the delay
        }
    }

    void EnableScript()
    {
        if (targetScript != null)
        {
            targetScript.enabled = true;
        }
    }
}
