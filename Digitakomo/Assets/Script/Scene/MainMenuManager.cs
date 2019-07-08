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

    private bool isMultiplayer = false;

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
        StartMenu.SetActive(false);
        // StartMapSelection.SetActive(true);
        isMultiplayer = false;
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }

    public void OnClick_MultiPlayer()
    {
        StartMenu.SetActive(false);
        // StartMapSelection.SetActive(true);
        isMultiplayer = true;
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }

    public void OnClick_Back_StartMenu()
    {
        StartMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion
}
