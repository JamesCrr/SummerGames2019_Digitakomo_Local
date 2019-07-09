using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInputManager : MonoBehaviour
{
    Event keyEvent;
    bool waitingForKey;
    KeyCode newKey;
    Text buttonText;
    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        // assign value to text 
        foreach (Button button in buttons)
        {
            string name = button.name.Split('-')[0];
            int index = Int16.Parse(button.name.Split('-')[1]);
            button.GetComponentInChildren<Text>().text = InputManager.GetKeyByName(name, index).ToString();
        }
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
        {
            yield return null;
        }
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey();

        // handle logic changing key
        string name = keyName.Split('-')[0];
        int index = Int16.Parse(keyName.Split('-')[1]);

        // set new key 
        InputManager.SetKey(name, newKey, index);

        // apply new key to button text
        buttonText.text = InputManager.GetKeyByName(name, index).ToString();
    }
}
