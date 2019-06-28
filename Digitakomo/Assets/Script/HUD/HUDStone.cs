using UnityEngine;
using UnityEngine.UI;

public class HUDStone : MonoBehaviour
{
    public Egg stone;
    private Text stoneHP;

    // Start is called before the first frame update
    void Start()
    {
        stoneHP = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        setStoneHPText(stone.GetCurrentHP().ToString());
    }

    void setStoneHPText(string HP)
    {
        stoneHP.text = HP;
    }
}
