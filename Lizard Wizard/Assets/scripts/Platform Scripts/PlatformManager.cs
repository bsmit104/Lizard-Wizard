using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject waterPrefab;
    public float levelWidth = 3f;
    public float minY = 1f;
    public float maxY = 2f;
    public float waterCheckInterval = 0.5f;
    public int maxPlatforms = 20;

    private Transform playerTransform;
    private float spawnThreshold = 10f;
    private float lastSpawnPositionY;
    private GameObject waterObject;
    private Queue<GameObject> platforms = new Queue<GameObject>();

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
        GameObject newPlatform = platforms.Dequeue();
        // stop water movement if present
        PlatformMovement platformMovement = newPlatform.GetComponent<PlatformMovement>();
        platformMovement.ResetMovement();
        // trigger chance to spawn coin
        PlatformCoin platformCoin = platformMovement.GetComponent<PlatformCoin>();
        platformCoin.SpawnCoin();

        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        newPlatform.transform.position = spawnPosition;

        platforms.Enqueue(newPlatform);
        lastSpawnPositionY = spawnPosition.y;
    }

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