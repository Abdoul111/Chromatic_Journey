using System.Collections;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] objectPrefabs; 
    public Vector2 spawnAreaSize = new Vector2(10f, 5f); 
    public int objectCount = 20; 
    public float spawnDensity = 1f; 

    [Header("Spawn Area Position")]
    public Transform spawnAreaCenter; 

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
        GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        Vector2 randomPosition = new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        Vector2 spawnPosition = (Vector2)spawnAreaCenter.position + randomPosition;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
