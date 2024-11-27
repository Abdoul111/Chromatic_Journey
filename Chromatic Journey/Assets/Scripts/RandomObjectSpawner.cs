using System.Collections;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] objectPrefabs; // Pool of objects for random spawning
    public GameObject fallingObjectPrefab; // Specific prefab for "FallingObject"
    public Vector2 spawnAreaSize = new Vector2(10f, 5f);
    public int objectCount = 20;
    public float spawnDensity = 1f;

    [Header("Spawn Area Position")]
    public Transform spawnAreaCenter;

    [Header("Trail Settings")]
    public bool enableTrail = true;
    public float trailTime = 0.5f;
    public Material trailMaterial;
    public Color trailStartColor = Color.white;
    public Color trailEndColor = new Color(1, 1, 1, 0);
    public float trailStartWidth = 0.5f;
    public float trailEndWidth = 0f;

    [Header("Particle System Settings")]
    public bool enableParticles = true;
    public ParticleSystem particlePrefab;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(spawnDensity);
        }
    }

    private void SpawnRandomObject()
    {
        // Check if a specific FallingObject prefab should be used
        GameObject prefabToSpawn = fallingObjectPrefab != null 
            ? fallingObjectPrefab 
            : objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        Vector2 randomPosition = new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );
        Vector2 spawnPosition = (Vector2)spawnAreaCenter.position + randomPosition;

        // Spawn the object
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Add trail renderer if enabled
        if (enableTrail)
        {
            TrailRenderer trail = spawnedObject.AddComponent<TrailRenderer>();
            SetupTrailRenderer(trail);
        }

        // Add particle system if enabled
        if (enableParticles && particlePrefab != null)
        {
            AttachParticleSystem(spawnedObject);
        }
    }

    private void SetupTrailRenderer(TrailRenderer trail)
    {
        trail.time = trailTime;
        trail.material = trailMaterial;
        trail.startColor = trailStartColor;
        trail.endColor = trailEndColor;
        trail.startWidth = trailStartWidth;
        trail.endWidth = trailEndWidth;
        trail.autodestruct = false;
    }

    private void AttachParticleSystem(GameObject target)
    {
        // Create particle system instance
        ParticleSystem particles = Instantiate(particlePrefab, target.transform.position, Quaternion.identity);

        // Make particle system a child of the spawned object
        particles.transform.SetParent(target.transform);

        // Reset position relative to parent
        particles.transform.localPosition = Vector3.zero;

        // Set up particle system to follow object
        ParticleSystem.MainModule main = particles.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
    }
}
