using UnityEngine;
using UnityEngine.UI;

public class coinHandler : MonoBehaviour
{
    public Text coinText;
    private int coinsCollected = 0;
    public AudioClip pickupSound;
    private AudioSource pickupAudioSource; // Dedicated AudioSource for pickup sounds
    public GameObject pickupParticle;

    private void Start()
    {
        // Add or get a dedicated AudioSource for pickup sounds
        pickupAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLLIDED:" + other);
        if (other.CompareTag("Coin"))
        {
            if (pickupParticle != null)
            {
                Instantiate(pickupParticle, other.transform.position, Quaternion.identity);
            }
            coinsCollected++;
            coinText.text = "x" + coinsCollected.ToString();
            if (pickupSound != null && pickupAudioSource != null)
            {
                pickupAudioSource.PlayOneShot(pickupSound);
            }
            Destroy(other.gameObject);
        }
    }
}
