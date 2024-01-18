using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Config")]
    public float spawnTime = 5.0f;
    private float timeSinceSpawn = 0.0f;
    public int spawnCount = 1;

    [Header("References")]
    [SerializeField]
    private GameObject enemyPrefab;
    private Collider spawnCollider;

    private void Awake()
    {
        spawnCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn > spawnTime)
        {
            SpawnEnemies();
            timeSinceSpawn = 0.0f;
        }
    }

    private void SpawnEnemies()
    {
        if (spawnCollider != null)
        {
            Bounds bounds = spawnCollider.bounds;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 randomPositionWithinBounds = GetRandomPosInBounds(bounds);
                Instantiate(enemyPrefab, randomPositionWithinBounds, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Collider not found on the spawner GameObject.");
        }
    }

    private Vector3 GetRandomPosInBounds(Bounds bounds)
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y) - 2.0f; // Hardcoded to move down to floor
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
