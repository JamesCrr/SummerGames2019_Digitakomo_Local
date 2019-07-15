using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private string[] actions = new string[15]
    {
        "Player1ChangeCharacter",
        "Player1LookUp",
        "Player1LookDown",
        "Player1Jump",
        "Player1SpecialAttack",
        "Player1LockMovement",
        "Player1MoveLeft",
        "Player1MoveRight",
        "Player2LookUp",
        "Player2LookDown",
        "Player2Jump",
        "Player2SpecialAttack",
        "Player2LockMovement",
        "Player2MoveLeft",
        "Player2MoveRight"
    };

    private KeyCode[,] defaultKeys = new KeyCode[15, 2]
    {
        { KeyCode.C, KeyCode.None },
        { KeyCode.W, KeyCode.None },
        { KeyCode.S, KeyCode.None },
        { KeyCode.J, KeyCode.None },
        { KeyCode.H, KeyCode.None },
        { KeyCode.LeftShift, KeyCode.None },
        { KeyCode.A, KeyCode.None },
        { KeyCode.D, KeyCode.None },
        { KeyCode.UpArrow, KeyCode.None },
        { KeyCode.DownArrow, KeyCode.None },
        { KeyCode.Keypad6, KeyCode.None },
        { KeyCode.Keypad5, KeyCode.None },
        { KeyCode.RightControl, KeyCode.None },
        { KeyCode.LeftArrow, KeyCode.None },
        { KeyCode.RightArrow, KeyCode.None }
    };

    private Dictionary<string, KeyCode[]> keymaps = new Dictionary<string, KeyCode[]>();

    private List<string> pressing = new List<string>();

    public static InputManager Instance;

    public bool UseDefaultKey = false;
    public InputDatabaseManager DB;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DB = new InputDatabaseManager(Application.dataPath);
        // initialize 
        Initialize();
        if (!UseDefaultKey)
        {
            // if database exist
            if (DB.IsDatabaseExist())
            {
                // load setting into variable
                DB.SetUpKeyFromDb();
            }
        }

    }

    public void Initialize()
    {
        // default value
        for (int i = 0; i < actions.Length; i++)
        {
            KeyCode[] keycodes = new KeyCode[2] { defaultKeys[i, 0], defaultKeys[i, 1] };
            keymaps.Add(actions[i], keycodes);
        }
    }

    private void Update()
    {
        foreach (string keyname in keymaps.Keys)
        {
            if (GetButtonDown(keyname) && !pressing.Contains(keyname))
            {
                pressing.Add(keyname);
            }
            else if (GetButtonUp(keyname) && pressing.Contains(keyname))
            {
                pressing.Remove(keyname);
            }
        }
    }

    public static bool GetButtonDown(string name)
    {
        bool isPressed = Input.GetKeyDown(InputManager.Instance.keymaps[name][0]) || Input.GetKeyDown(InputManager.Instance.keymaps[name][1]);
        return isPressed;
    }

    public static bool GetButtonUp(string name)
    {
        bool isPressed = Input.GetKeyUp(InputManager.Instance.keymaps[name][0]) || Input.GetKeyUp(InputManager.Instance.keymaps[name][1]);
        return isPressed;
    }

    public static float GetAxisRaw(string name)
    {
        return InputManager.Instance.pressing.Contains(name) ? 1 : 0;
    }

    public static void SetKeyNoSave(string action, KeyCode newKey, int index)
    {
        if (!InputManager.Instance.keymaps.ContainsKey(action))
        {
            throw new Exception("Key does not contain action " + action);
        }
        InputManager.Instance.keymaps[action][index] = newKey;
    }

    public static void SetKey(string action, KeyCode newKey, int index)
    {
        SetKeyNoSave(action, newKey, index);
        // save to database
        InputManager.Instance.DB.SaveKeyToDb();
    }

    public static Dictionary<string, KeyCode[]> GetKeys()
    {
        return InputManager.Instance.keymaps;
    }

    public static KeyCode GetKeyByName(string name, int index)
    {
        return InputManager.Instance.keymaps[name][index];
    }

    public static void RestoreDefault()
    {
        InputManager.Instance.keymaps.Clear();
        InputManager.Instance.Initialize();
        InputManager.Instance.DB.SaveKeyToDb();
    }
}
