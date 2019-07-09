using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject StartMenu;
    public GameObject StartMapSelection;
    public GameObject StartReady;
    public GameObject ExtraMenu;
    public GameObject ExtraArt;
    public GameObject ExtraCharacter;
    public GameObject SettingMenu;
    public GameObject CreditMenu;

    #region MainMenu
    public void OnClick_Start()
    {
        MainMenu.SetActive(false);
        StartMenu.SetActive(true);
    }

    public void OnClick_Extra()
    {
        MainMenu.SetActive(false);
        ExtraMenu.SetActive(true);
    }

    public void OnClick_Setting()
    {
        MainMenu.SetActive(false);
        SettingMenu.SetActive(true);
    }

    public void OnClick_Credit()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }
    #endregion

    #region StartMenu
    public void OnClick_SinglePlayer()
    {
        // StartMenu.SetActive(false);
        // StartMapSelection.SetActive(true);
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }

    public void OnClick_MultiPlayer()
    {
        // StartMenu.SetActive(false);
        // StartMapSelection.SetActive(true);
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }

    public void OnClick_Back_StartMenu()
    {
        StartMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion

    #region ExtraMenu
    public void OnClick_Character_Extra()
    {
        ExtraMenu.SetActive(false);
        ExtraCharacter.SetActive(true);
    }

    public void OnClick_Back_Extra()
    {
        ExtraMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion 

    #region ExtraCharacter

    public void OnClick_Back_Extra_Character()
    {
        ExtraMenu.SetActive(true);
        ExtraCharacter.SetActive(false);
    }
    #endregion 
}
