using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText("Score: " + PlayerManager.Instance.GetPlayerScore());
    }

    public void OnClick_Restart() 
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

        PlayerManager.Instance.RestartGame();
    }
    public void OnClick_MainMenu() 
    {
        Time.timeScale = 1;
        MenuManager.OpenMenu(Menu.MAIN_MENU, null);
        gameObject.SetActive(false);

        PlayerManager.Instance.QuitGame();
    }
}