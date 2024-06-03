using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    //[SerializeField] private GameObject player;

    private PlayerHealth playerHealth;
    private PlayerScore playerScore;

    void Awake() 
    {
        if (Instance != null && Instance != this) 
            Destroy(gameObject);
        else 
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = GetComponent<PlayerScore>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10)
        {
            EndGame();
        }
    }

    public void AddPlayerScore(float score) {
        playerScore.AddScore(score);
    }

    public void ChangePlayerHealth(int healthChange) {
        playerHealth.ChangeHealth(healthChange);
    }

    public void EndGame() {
        // end game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
