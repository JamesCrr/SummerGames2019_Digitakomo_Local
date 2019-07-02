using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    InputDatabaseManager idm;
    // Start is called before the first frame update
    void Start()
    {
        idm = new InputDatabaseManager(Application.dataPath);

        Dictionary<string, KeyCode[]> keymaps = InputManager.GetKeys();

        string saved = "";
        foreach (string key in keymaps.Keys)
        {
            string value1 = keymaps[key][0].ToString();
            string value2 = keymaps[key][1].ToString();

            saved += key + "=" + value1 + "," + value2 + "\n";
            //Debug.Log(key);
            //Debug.Log((KeyCode)System.Enum.Parse(typeof(KeyCode), keymaps[key][0].ToString()));
        }

        idm.WriteLine(saved);
        //Debug.Log(saved);
    }
}
