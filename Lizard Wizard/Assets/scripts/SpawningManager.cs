using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public GameObject platformPrefab; // Second platform prefab
    public GameObject waterPrefab;
    public GameObject libraryBackground; // Background object to activate
    public float levelWidth = 5f;
    public float minY = 1f;
    public float maxY = 2f;
    public float waterCheckInterval = 0.5f;
    public int maxPlatforms = 20;

    private Transform playerTransform;
    private float spawnThreshold = 10f;
    private float lastSpawnPositionY;
    private GameObject waterObject;
    private Queue<GameObject> platforms = new Queue<GameObject>();

    public List<GameObject> obstacles;
    public GameObject enemyPrefab;
    private int platformsToEnemy = 1;
    private float averagePlatformsToEnemy = 10f;
    private EnemyBehaviour behaviourScript;
    private int platformCount = 0; // Track number of platforms spawned

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastSpawnPositionY = playerTransform.position.y;

        SpawnInitialPlatforms();
        //SpawnInitialEnemies();

        waterObject = GameObject.FindGameObjectWithTag("Water").gameObject;
        // Instantiate the water object if it doesn't already exist
        if (waterObject == null)
        {
            waterObject = Instantiate(waterPrefab, new Vector3(0, -5f, 0), Quaternion.identity);
        }

        StartCoroutine(CheckWaterLevel());
    }

    void Update()
    {
        if (playerTransform.position.y > lastSpawnPositionY - spawnThreshold)
        {
            SpawnPlatform();
        }
    }

    void SpawnInitialPlatforms()
    {
        GameObject newPlatform = Instantiate(platformPrefab, playerTransform.position - new Vector3(0.25f, 1.25f, 0f), Quaternion.identity);
        platforms.Enqueue(newPlatform);
        newPlatform.transform.SetParent(GameObject.Find("Platforms").transform);
        platformCount++;

        Vector3 spawnPosition = new Vector3();
        for (int i = 1; i < maxPlatforms; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platforms.Enqueue(newPlatform);
            lastSpawnPositionY = spawnPosition.y;
            platformCount++;

            newPlatform.transform.SetParent(GameObject.Find("Platforms").transform);
        }
    }

    void SpawnInitialEnemies(){
        Vector3 spawnPoint = new Vector3(0,-30,0);
        for (int i = 0; i < 6; i+=1)
        {  
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            obstacles.Add(newEnemy);
        }
    }

    void SpawnPlatform()
    {
        if (platforms.Count == 0) return; // Ensure there are platforms in the queue

        GameObject newPlatform = platforms.Dequeue();
        platformsToEnemy -= 1;

        // stop water movement if present
        PlatformMovement platformMovement = newPlatform.GetComponent<PlatformMovement>();
        platformMovement.ResetMovement();

        // trigger chance to spawn coin
        PlatformCoin platformCoin = platformMovement.GetComponent<PlatformCoin>();
        platformCoin.SpawnCoin();

        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);

        if (platformsToEnemy <= 0)
        {
            if (obstacles.Count > 0)
            {
                int behaviourIndex = Random.Range(1, 5);
                GameObject enemy = obstacles[behaviourIndex-1];
                behaviourScript = enemy.GetComponent<EnemyBehaviour>();

                Vector3 enemyPosition = new Vector3();
                enemyPosition.y = spawnPosition.y;
                enemyPosition.x = ((spawnPosition.x > 0) ? -levelWidth : 0) + Random.Range(0, levelWidth);

                behaviourScript.changeBehaviour(behaviourIndex);
                behaviourScript.changePosition(enemyPosition);

                platformsToEnemy = Mathf.RoundToInt(Random.Range(0.7f * averagePlatformsToEnemy, 1.3f * averagePlatformsToEnemy));
            }
            else
            {
                Debug.LogWarning("No obstacles available to spawn.");
            }
        }

        newPlatform.transform.position = spawnPosition;
        platforms.Enqueue(newPlatform);
        lastSpawnPositionY = spawnPosition.y;
        platformCount++;
    }

    IEnumerator CheckWaterLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(waterCheckInterval);
            foreach (GameObject platform in platforms)
            {
                if (platform.transform.position.y < waterObject.transform.position.y + 12f)
                {
                    PlatformMovement platformMovement = platform.GetComponent<PlatformMovement>();
                    if (platformMovement != null)
                    {
                        platformMovement.Engulf();
                    }
                }
            }
        }
    }
}