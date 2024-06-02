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
    //private List<GameObject> platforms = new List<GameObject>();
    //private GameObject[] platforms;
    //private int platformIterator = 0;
    private Queue<GameObject> platforms = new Queue<GameObject>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastSpawnPositionY = playerTransform.position.y;

        //platforms = new GameObject[maxPlatforms];
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
        // Vector3 spawnPosition = new Vector3();
        // for (int i = 0; i < 10; i++)
        // {
        //     spawnPosition.y += Random.Range(minY, maxY);
        //     spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        //     GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        //     platforms.Add(newPlatform);
        //     lastSpawnPositionY = spawnPosition.y;
        // }

        // Spawn initial platform below player

        // platforms[0] = Instantiate(platformPrefab, playerTransform.position - new Vector3(0.25f, 1.25f, 0f), Quaternion.identity);
        // platforms[0].transform.SetParent(GameObject.Find("Platforms").transform);

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
        // Vector3 spawnPosition = new Vector3();
        // spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
        // spawnPosition.x = Random.Range(-levelWidth, levelWidth);
        // GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        // platforms.Add(newPlatform);
        // lastSpawnPositionY = spawnPosition.y;

        GameObject newPlatform = platforms.Dequeue();
        Debug.Log("Dequeuing at "+ newPlatform.transform.position.y);
        PlatformMovement platformMovement = newPlatform.GetComponent<PlatformMovement>();
        platformMovement.ResetMovement();

        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = lastSpawnPositionY + Random.Range(minY, maxY);
        spawnPosition.x = Random.Range(-levelWidth, levelWidth);

        newPlatform.transform.position = spawnPosition;
        platforms.Enqueue(newPlatform);
        //newPlatform.SetActive(true);
        lastSpawnPositionY = spawnPosition.y;
        Debug.Log("Spawning at " + spawnPosition.y);
        
        //platformIterator = (platformIterator + 1) % maxPlatforms;
        
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
                if (platform.transform.position.y < waterObject.transform.position.y + 4.73f)
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