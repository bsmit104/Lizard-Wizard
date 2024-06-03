using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDetection : MonoBehaviour
{
    [SerializeField] private float coinScore = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            //add to player score
            PlayerManager.Instance.AddPlayerScore(coinScore);
            gameObject.SetActive(false);
        }
    }
}
