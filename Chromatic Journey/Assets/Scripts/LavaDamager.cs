using UnityEngine;

public class LavaDamage2D : MonoBehaviour
{
    public float damagePerSecond = 10f; // Damage applied to the player per second

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object is the player
        if (other.CompareTag("Player"))
        {
            HealthCounter.Damage(10 * Time.deltaTime);
            Debug.Log("Player Damaged");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the lava!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the lava!");
        }
    }
}
