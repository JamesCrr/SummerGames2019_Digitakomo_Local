using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class InputDatabaseManager
{
    string path;
    readonly string filename = "/Resources/UserSetting.dgkm";

    public InputDatabaseManager(string ApplicationPath)
    {
        path = ApplicationPath;
    }

    public void WriteLine(string text)
    {
        File.WriteAllText(path + filename, text);
    }

    public bool SaveKeyToDb()
    {
        try
        {
            Debug.Log("Saving input to database");
            Dictionary<string, KeyCode[]> keys = InputManager.GetKeys();

            string saveTexts = "";
            foreach (string key in keys.Keys)
            {
                KeyCode mainKey = keys[key][0];
                KeyCode altKey = keys[key][1];

                string keyName = key;

                // main key format (Player1MoveLeft-0=A)
                saveTexts += keyName + "-0=" + mainKey.ToString() + "\n";
                saveTexts += keyName + "-1=" + altKey.ToString() + "\n";
            }

            WriteLine(saveTexts);

            Debug.Log("Saved input to database");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public bool SetUpKeyFromDb()
    {
        try
        {
            if (File.Exists(path + filename))
            {
                // Read a text file line by line.
                string[] lines = File.ReadAllLines(path + filename);
                foreach (string line in lines)
                {
                    string[] splitTexts = line.Split('=');
                    // if match our format
                    if (splitTexts.Length >= 2)
                    {
                        string namestring = splitTexts[0];
                        string keycode = splitTexts[1];

                        string[] namestringSplit = namestring.Split('-');
                        string keyName = namestringSplit[0];
                        int index = Int16.Parse(namestringSplit[1]);

                        KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), keycode);

                        InputManager.SetKeyNoSave(keyName, key, index);
                    }
                }
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public bool IsDatabaseExist()
    {
        return File.Exists(path + filename);
    }
}
