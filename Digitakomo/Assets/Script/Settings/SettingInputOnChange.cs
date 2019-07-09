using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInputOnChange : MonoBehaviour
{
    InputField inputField;

    private void Start()
    {
        inputField = GetComponent<InputField>();
    }

    public void OnChanged()
    {
        string currentKey = null;
        string text = inputField.text;
        if (text.Length > 0)
            currentKey = inputField.text[text.Length - 1].ToString().ToUpper();

        inputField.text = currentKey;
    }

}
