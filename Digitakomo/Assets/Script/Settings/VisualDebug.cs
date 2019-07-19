using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualDebug : MonoBehaviour
{
    public Text text;
    public static VisualDebug Instance;

    private List<string> debugs;

    public int Line;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        debugs = new List<string>();
        Instance = this;
    }

    private void FixedUpdate()
    {
        string visible = "";
        foreach (string debug in debugs.ToArray())
        {
            visible += debug + "\n";
        }
        text.text = visible;
    }

    public void Log(string text)
    {
        if (debugs.Count > Line)
        {
            debugs.RemoveAt(0);
        }
        debugs.Add(text);
    }
}
