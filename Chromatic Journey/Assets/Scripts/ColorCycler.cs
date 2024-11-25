using UnityEngine;

public class ColorCycler : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Assign the SpriteRenderer in the Inspector
    public float cycleDuration = 3f; // Time it takes to complete one color cycle

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(CycleColors());
        }
        else
        {
            Debug.LogError("SpriteRenderer is not assigned or found!");
        }
    }

    private System.Collections.IEnumerator CycleColors()
    {
        while (true)
        {
            // Transition from Red to Green
            yield return StartCoroutine(TransitionColor(Color.red, Color.green, cycleDuration / 3f));

            // Transition from Green to Blue
            yield return StartCoroutine(TransitionColor(Color.green, Color.blue, cycleDuration / 3f));

            // Transition from Blue to Red
            yield return StartCoroutine(TransitionColor(Color.blue, Color.red, cycleDuration / 3f));
        }
    }

    private System.Collections.IEnumerator TransitionColor(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate between startColor and endColor
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final color is set
        spriteRenderer.color = endColor;
    }
}
