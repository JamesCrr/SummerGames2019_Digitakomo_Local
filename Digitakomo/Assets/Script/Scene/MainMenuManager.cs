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
    public GameObject ExtraCharacterFay;
    public GameObject ExtraCharacterBag;
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
        StartMenu.SetActive(false);
        StartMapSelection.SetActive(true);

        FindObjectOfType<CrossScene>().PlayerCount = 1;
    }

    public void OnClick_MultiPlayer()
    {
        StartMenu.SetActive(false);
        StartMapSelection.SetActive(true);

        FindObjectOfType<CrossScene>().PlayerCount = 2;
    }

    public void OnClick_Back_StartMenu()
    {
        StartMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion

    #region MapSelection

    public void OnClick_Basic()
    {
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }
    public void OnClick_Advance()
    {
        SceneController._LoadSceneWithLoadingScreen("RaviossaLevel");
    }

    public void OnClick_Back_StartMapSelection()
    {
        StartMapSelection.SetActive(false);
        StartMenu.SetActive(true);
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

    public void OnClick_Extra_Fay()
    {
        ExtraCharacterFay.SetActive(true);
        ExtraCharacter.SetActive(false);
    }

    public void OnClick_Extra_Bag()
    {
        ExtraCharacterBag.SetActive(true);
        ExtraCharacter.SetActive(false);
    }

    public void OnClick_Back_Extra_Fay()
    {
        ExtraCharacter.SetActive(true);
        ExtraCharacterFay.SetActive(false);
    }

    public void OnClick_Back_Extra_Bag()
    {
        ExtraCharacter.SetActive(true);
        ExtraCharacterBag.SetActive(false);
    }
    #endregion 

    #region SettingMenu
    public void OnClick_Back_Setting()
    {
        SettingMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion
}
