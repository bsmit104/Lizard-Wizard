using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Update() 
    {
        
    }

    public void OnClick_Resume() 
    {
        PlayerManager.Instance.toggleGameHUD(true);
        UnPause();
    }
    public void OnClick_Options() 
    {
        MenuManager.OpenMenu(Menu.OPTIONS, null);
    }
    public void OnClick_MainMenu() 
    {
        MenuManager.OpenMenu(Menu.MAIN_MENU, null);
        PlayerManager.Instance.QuitGame();
        UnPause();
    }

    private void UnPause() 
    {
        Time.timeScale = 1;
        PlayerManager.Instance.SetPauseStatus(false);
        gameObject.SetActive(false);
    }
}
