using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject waterPrefab;
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
    private int platformsToEnemy = 1;
    private float averagePlatformsToEnemy = 15f;
    private EnemyBehaviour behaviourScript;


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastSpawnPositionY = playerTransform.position.y;

        SpawnInitialPlatforms();

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

        Vector3 spawnPosition = new Vector3();
        for (int i = 1; i < maxPlatforms; i++)
        {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platforms.Enqueue(newPlatform);
            lastSpawnPositionY = spawnPosition.y;

            newPlatform.transform.SetParent(GameObject.Find("Platforms").transform);
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
            GameObject enemy = obstacles[Random.Range(0, obstacles.Count)];
            behaviourScript = enemy.GetComponent<EnemyBehaviour>();

            Vector3 enemyPosition = new Vector3();
            enemyPosition.y = spawnPosition.y;
            enemyPosition.x = ((spawnPosition.x > 0) ? -levelWidth : 0) + Random.Range(0, levelWidth);

            int behaviourIndex = Random.Range(1, 3);
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
}
    // void SpawnPlatform()
    // {
    //     GameObject newPlatform = platforms.Dequeue();
    //     platformsToEnemy -= 1;
    //     // stop water movement if present
    //     PlatformMovement platformMovement = newPlatform.GetComponent<PlatformMovement>();
    //     platformMovement.ResetMovement();
    //     // trigger chance to spawn coin
    //     PlatformCoin platformCoin = platformMovement.GetComponent<PlatformCoin>();
    //     platformCoin.SpawnCoin();

    //     Vector3 spawnPosition = new Vector3();
    //     spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
    //     spawnPosition.x = Random.Range(-levelWidth, levelWidth);

    //     if (platformsToEnemy <= 0){
    //         GameObject enemy = obstacles[Random.Range(0,obstacles.Count)];
    //         behaviourScript = enemy.GetComponent<EnemyBehaviour>();
    //         Vector3 enemyPosition = new Vector3();
    //         enemyPosition.y = spawnPosition.y;
    //         enemyPosition.x = ((spawnPosition.x > 0) ? -levelWidth : 0) + Random.Range(0, levelWidth);
    //         int behaviourIndex = Random.Range(1,3);
    //         //Debug.Log("new enemy behaviour is: "+behaviourIndex);
    //         behaviourScript.changeBehaviour(behaviourIndex);
    //         behaviourScript.changePosition(enemyPosition);
    //         platformsToEnemy = Mathf.RoundToInt(Random.Range(0.7f*averagePlatformsToEnemy , 1.3f*averagePlatformsToEnemy));
    //     }
    //     newPlatform.transform.position = spawnPosition;

    //     platforms.Enqueue(newPlatform);
    //     lastSpawnPositionY = spawnPosition.y;
    // }

    IEnumerator CheckWaterLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(waterCheckInterval);
            //Debug.Log("Checking water level... Water Y position: " + waterObject.transform.position.y);
            foreach (GameObject platform in platforms)
            {
                //Debug.Log("Platform Y position: " + platform.transform.position.y);
                if (platform.transform.position.y < waterObject.transform.position.y + 12f)
                {
                    PlatformMovement platformMovement = platform.GetComponent<PlatformMovement>();
                    if (platformMovement != null)
                    {
                        //Debug.Log(platform.name + " is below the water level.");
                        platformMovement.Engulf();
                    }
                }
            }
        }
    }
}