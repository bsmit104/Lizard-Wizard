using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public float levelWidth = 3f;
    public float minY = 1f;
    public float maxY = 2f;

    private Transform playerTransform;
    private float spawnThreshold = 10f; // how far ahead to spawn platforms
    private float lastSpawnPositionY;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastSpawnPositionY = playerTransform.position.y;

        SpawnInitialPlatforms();
    }

    void Update()
    {
        // Check if the player has moved up enough, spawn more platforms
        if (playerTransform.position.y > lastSpawnPositionY - spawnThreshold)
        {
            SpawnPlatform();
        }
    }

    void SpawnInitialPlatforms()
    {
        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < 10; i++) // determine how many initial platforms to spawn
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            lastSpawnPositionY = spawnPosition.y;
        }
    }

    void SpawnPlatform()
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlatformManager : MonoBehaviour
// {
//     public GameObject platformPrefab;
//     public int numberOfPlatforms = 10;
//     public float levelWidth = 3f;
//     public float minY = 1f;
//     public float maxY = 2f;

//     void Start()
//     {
//         Debug.Log("P Script started");
//         Vector3 spawnPosition = new Vector3();

//         for (int i = 0; i < numberOfPlatforms; i++)
//         {
//             spawnPosition.y += Random.Range(minY, maxY);
//             spawnPosition.x = Random.Range(-levelWidth, levelWidth);
//             Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
//         }
//     }
// }
