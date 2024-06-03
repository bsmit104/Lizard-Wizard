using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager1 : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    public float spawnDistanceAbovePlayer = 10f; // Distance above player to spawn background
    private Transform playerTransform;
    private float lastSpawnPositionY;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastSpawnPositionY = playerTransform.position.y;
        SpawnInitialBackground();
    }

    void Update()
    {
        // Check if the player has moved up enough to spawn a new background
        if (playerTransform.position.y > lastSpawnPositionY - spawnDistanceAbovePlayer)
        {
            SpawnBackground();
        }
    }

    void SpawnInitialBackground()
    {
        Vector3 spawnPosition = new Vector3(0, playerTransform.position.y + spawnDistanceAbovePlayer, 0);
        Instantiate(BackgroundPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;
    }

    void SpawnBackground()
    {
        Vector3 spawnPosition = new Vector3(0, lastSpawnPositionY + spawnDistanceAbovePlayer, 0);
        Instantiate(BackgroundPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;
    }
}
