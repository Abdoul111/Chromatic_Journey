using UnityEngine;
using UnityEngine.UI;

public class coinHandler : MonoBehaviour
{
    public Text coinText;
    private int coinsCollected = 0;
    public AudioClip pickupSound;
    private AudioSource audioSource;
    public GameObject pickupParticle;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLLIDED:" + other);
        if (other.CompareTag("Coin"))
        {
            if (pickupParticle != null)
            {
                Instantiate(pickupParticle, other.transform.position, Quaternion.identity);
                Destroy(pickupParticle, 2f);
            }
            coinsCollected++;
            coinText.text = "x" + coinsCollected.ToString();
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            Destroy(other.gameObject);
        }
    }
}
