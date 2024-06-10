using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void OnClick_Back() 
    {
        if (!PlayerManager.isPlaying) 
            MenuManager.OpenMenu(Menu.MAIN_MENU, null);
            
        MenuManager.CloseMenu(Menu.OPTIONS);

    }
}
