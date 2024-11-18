using UnityEngine;
public class LavaDamage2D : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damagePerSecond = 10f;
    [Header("Visual Effects")]
    public ParticleSystem lavaParticlesPrefab;
    private ParticleSystem activeParticles;
    [Header("Particle Settings")]
    public float particleDensity = 10f;
    public float particleLifetime = 2f;

    private void Start()
    {
        if (lavaParticlesPrefab != null)
        {
            SetupParticleSystem();
        }
    }

    private void SetupParticleSystem()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null) return;

        if (activeParticles != null)
        {
            Destroy(activeParticles.gameObject);
        }

        // Create particle system as child of this object
        activeParticles = Instantiate(lavaParticlesPrefab, transform);

        // Calculate position to be at the top of the lava
        float lavaHeight = collider.bounds.size.y;
        float lavaWidth = collider.bounds.size.x;

        // Position at the top of the lava
        activeParticles.transform.localPosition = new Vector3(0f, lavaHeight / 3 , 0f);
        activeParticles.transform.localRotation = Quaternion.identity;

        // Scale width based on lava width, but keep the specific height scale
        activeParticles.transform.localScale = new Vector3(0.25f * lavaWidth, 0.25f, 2f);

        // Configure particle system
        var main = activeParticles.main;
        main.startLifetime = particleLifetime;

        var shape = activeParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = Vector3.one;  // Using Vector3.one since we're controlling scale via transform

        var emission = activeParticles.emission;
        emission.rateOverTime = lavaWidth * particleDensity;

        activeParticles.Play();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthCounter.Damage(10 * Time.deltaTime);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying && activeParticles != null)
        {
            SetupParticleSystem();
        }
    }
}