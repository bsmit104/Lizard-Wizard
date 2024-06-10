using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void OnClick_Start() 
    {
        MenuManager.CloseMenu(Menu.MAIN_MENU);
    }
    public void OnClick_Options() 
    {
        MenuManager.OpenMenu(Menu.OPTIONS, gameObject);
    }
    public void OnClick_Credits() 
    {
        MenuManager.OpenMenu(Menu.CREDITS, gameObject);
    }
    public void OnClick_Quit() 
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
