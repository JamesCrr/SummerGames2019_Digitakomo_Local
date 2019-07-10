using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private string[] actions = new string[17]
    {
        "Player1ChangeCharacter",
        "Player1LookUp",
        "Player1Jump",
        "Player1Attack",
        "Player1SpecialAttack",
        "Player1Defense",
        "Player1LockMovement",
        "Player1MoveLeft",
        "Player1MoveRight",
        "Player2LookUp",
        "Player2Jump",
        "Player2Attack",
        "Player2SpecialAttack",
        "Player2Defense",
        "Player2LockMovement",
        "Player2MoveLeft",
        "Player2MoveRight"
    };

    private KeyCode[,] keys = new KeyCode[17, 2]
    {
        { KeyCode.C, KeyCode.None },
        { KeyCode.W, KeyCode.None },
        { KeyCode.U, KeyCode.I },
        { KeyCode.H, KeyCode.None },
        { KeyCode.J, KeyCode.None },
        { KeyCode.E, KeyCode.None },
        { KeyCode.LeftShift, KeyCode.None },
        { KeyCode.A, KeyCode.None },
        { KeyCode.D, KeyCode.None },
        { KeyCode.UpArrow, KeyCode.None },
        { KeyCode.Keypad8, KeyCode.Keypad9 },
        { KeyCode.Keypad4, KeyCode.None },
        { KeyCode.Keypad5, KeyCode.Keypad6 },
        { KeyCode.Keypad7, KeyCode.None },
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
        DB = new InputDatabaseManager(Application.dataPath);
        if (Instance == null)
        {
            Instance = this;
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
    }

    public void Initialize()
    {
        // default value
        for (int i = 0; i < actions.Length; i++)
        {
            KeyCode[] keycodes = new KeyCode[2] { keys[i, 0], keys[i, 1] };
            keymaps.Add(actions[i], keycodes);
        }
    }

    public static bool GetButtonDown(string name)
    {
        bool isPressed = Input.GetKeyDown(InputManager.Instance.keymaps[name][0]) || Input.GetKeyDown(InputManager.Instance.keymaps[name][1]);
        if (isPressed && !InputManager.Instance.pressing.Contains(name))
        {
            InputManager.Instance.pressing.Add(name);
        }
        return isPressed;
    }

    public static bool GetButtonUp(string name)
    {
        bool isPressed = Input.GetKeyUp(InputManager.Instance.keymaps[name][0]) || Input.GetKeyUp(InputManager.Instance.keymaps[name][1]);
        if (isPressed && InputManager.Instance.pressing.Contains(name))
        {
            InputManager.Instance.pressing.Remove(name);
        }
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
}
