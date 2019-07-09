using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInputManager : MonoBehaviour
{
    Color oldColor;
    public void OnSelect(InputField selected)
    {
        Image component = selected.GetComponent<Image>();
        oldColor = component.color;
        component.color = Color.black;
    }

    public void OnDeselect(InputField deselected)
    {
        deselected.GetComponent<Image>().color = oldColor;
    }
}
