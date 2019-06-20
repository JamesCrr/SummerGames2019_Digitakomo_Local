using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Character character;
    private Text _HP;
    private Text _SP;

    // Start is called before the first frame update
    void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();

        foreach (Text text in texts)
        {
            switch (text.name)
            {
                case "HP": _HP = text; break;
                case "SP": _SP = text; break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetHealthText(character.GetCurrentHP().ToString());
        SetEnergyText(character.GetCurrentMP().ToString());
    }

    void SetHealthText(string text)
    {
        if (text == _HP.text)
        {
            return;
        }
        _HP.text = text;
    }

    void SetEnergyText(string text)
    {
        if (text == _SP.text)
        {
            return;
        }
        _SP.text = text;
    }
}
