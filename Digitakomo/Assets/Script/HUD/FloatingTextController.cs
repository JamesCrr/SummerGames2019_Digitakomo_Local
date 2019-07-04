using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
    }

    public static void CreateFloatingText(string text, Vector3 location)
    {
        FloatingText instance = ObjectPooler.Instance.FetchGO("PopupTextParent").GetComponent<FloatingText>();
        instance.transform.SetParent(canvas.transform, false);
        instance.SetText(text, location);
    }
}
