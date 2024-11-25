using UnityEngine;
using UnityEngine.UI;

public class coinHandler : MonoBehaviour
{
    public Text coinText;
    private int coinsCollected = 0;
    public int totalCoins = 0; // Total number of coins in the scene
    public AudioClip pickupSound;
    private AudioSource pickupAudioSource; // Dedicated AudioSource for pickup sounds
    public GameObject pickupParticle;

    private void Start()
    {
        // Add or get a dedicated AudioSource for pickup sounds
        pickupAudioSource = gameObject.AddComponent<AudioSource>();
        
        // Initialize the coin count display
        coinText.text = "x" + coinsCollected.ToString() + "/" + totalCoins.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (pickupParticle != null)
            {
                Instantiate(pickupParticle, other.transform.position, Quaternion.identity);
            }
            coinsCollected++;
            coinText.text = "x" + coinsCollected.ToString() + "/" + totalCoins.ToString();
            if (pickupSound != null && pickupAudioSource != null)
            {
                pickupAudioSource.PlayOneShot(pickupSound);
            }
            Destroy(other.gameObject);
        }
    }

    // Method to check if all coins are collected
    public bool AllCoinsCollected()
    {
        return coinsCollected >= totalCoins;
    }
}
