using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static string[] actions = new string[17]
    {
        "Player1ChangeCharcter",
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

    private static KeyCode[,] keys = new KeyCode[17, 2]
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

    private static Dictionary<string, KeyCode[]> keymaps = new Dictionary<string, KeyCode[]>();

    private static List<string> pressing = new List<string>();

    public static void Initialize(int player)
    {
        // if( noting in database )
        // default value
        for (int i = 0; i < actions.Length; i++)
        {
            if (!actions[i].Contains("Player" + player))
            {
                continue;
            }
            KeyCode[] keycodes = new KeyCode[2] { keys[i, 0], keys[i, 1] };
            keymaps.Add(actions[i], keycodes);
        }
    }

    public static bool GetButtonDown(string name)
    {
        bool isPressed = Input.GetKeyDown(keymaps[name][0]) || Input.GetKeyDown(keymaps[name][1]);
        if (isPressed && !pressing.Contains(name))
        {
            pressing.Add(name);
        }
        return isPressed;
    }

    public static bool GetButtonUp(string name)
    {
        bool isPressed = Input.GetKeyUp(keymaps[name][0]) || Input.GetKeyUp(keymaps[name][1]);
        if (isPressed && pressing.Contains(name))
        {
            pressing.Remove(name);
        }
        return isPressed;
    }

    public static float GetAxisRaw(string name)
    {
        return pressing.Contains(name) ? 1 : 0;
    }

    public static void SetKey(string action, KeyCode[] newKey)
    {
        if(!keymaps.ContainsKey(action))
        {
            throw new Exception("Key does not contain action " + action);
        }
        keymaps[action] = newKey;
    }
}
