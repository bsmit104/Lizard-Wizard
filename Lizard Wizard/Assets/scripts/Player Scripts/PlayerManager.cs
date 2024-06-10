using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static bool isPlaying { get; private set; }
    [SerializeField] private GameObject pauseMenu, gameOverMenu;

    private bool isPaused;
    private Canvas gameHUD;
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
        gameHUD = GameObject.Find("Game HUD").GetComponent<Canvas>();
        playerScore = GetComponent<PlayerScore>();
        playerHealth = GetComponent<PlayerHealth>();
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10)
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            SetPauseStatus(true);
            toggleGameHUD(false);

            pauseMenu.SetActive(true);
        }
    }

    public void toggleGameHUD(bool activeStatus) 
    {
        gameHUD.enabled = activeStatus;
    }

    public float GetPlayerScore() 
    {
        return playerScore.GetCurrentScore();
    }

    public void SetPauseStatus(bool pauseStatus) 
    {
         if (pauseStatus)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void AddPlayerScore(float score)
    {
        playerScore.AddScore(score);
    }

    public void ChangePlayerHealth(int healthChange)
    {
        playerHealth.ChangeHealth(healthChange);
    }

    public void EndGame()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() 
    {
        isPlaying = false;
        playerScore.ResetScore();
    }

}
