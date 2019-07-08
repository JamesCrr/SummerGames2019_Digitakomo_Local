using UnityEngine;
using UnityEngine.UI;

public class HUDStone : MonoBehaviour
{
    public Egg stone;
    public Slider hpSlider;
    // Update is called once per frame
    void Update()
    {
        hpSlider.value = stone.GetCurrentHP() / stone.MaxHP;
    }
}
