using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCoin : MonoBehaviour
{
    [SerializeField] private float coinChance = 0.25f;
    [SerializeField] private GameObject coinObj;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCoin() {
        if (Random.Range(0f, 1f) <= coinChance) {
            coinObj.gameObject.SetActive(true);
        }
    }

}
