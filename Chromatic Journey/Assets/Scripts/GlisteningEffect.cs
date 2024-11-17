using UnityEngine;

public class GlisteningEffect : MonoBehaviour
{
    private Material material;

    [Header("Glistening Settings")]
    public float glistenSpeed = 2f; // Speed of the glistening effect
    public float glistenIntensity = 0.5f; // Maximum intensity of the glisten
    public float pauseDuration = 1f; // Pause duration between glistens

    private float timer = 0f; // Timer to track the effect
    private bool isGlistening = true; // Whether the glisten effect is active

    private void Start()
    {
        // Get the material
        material = GetComponent<SpriteRenderer>().material;

        // Ensure the material has the required property
        if (!material.HasProperty("_GlistenIntensity"))
        {
            Debug.LogError("Material is missing _GlistenIntensity property!");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isGlistening)
        {
            // During the glistening phase
            float intensity = Mathf.Abs(Mathf.Sin(timer * glistenSpeed)) * glistenIntensity;
            material.SetFloat("_GlistenIntensity", intensity);

            // End glisten phase after one cycle
            if (timer >= Mathf.PI / glistenSpeed) // One full sine wave cycle
            {
                isGlistening = false;
                timer = 0f; // Reset timer for pause
            }
        }
        else
        {
            // During the pause phase
            material.SetFloat("_GlistenIntensity", 0f); // Turn off glisten effect

            if (timer >= pauseDuration)
            {
                isGlistening = true;
                timer = 0f; // Reset timer for next glisten cycle
            }
        }
    }
}
