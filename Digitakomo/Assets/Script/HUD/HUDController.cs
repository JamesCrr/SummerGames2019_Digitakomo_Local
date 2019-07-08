using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Character character;
    public Slider hpSlider, energySlider;

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = character.GetCurrentHP() / character.MaxHP;
        energySlider.value = character.GetCurrentMP() / character.MaxEnergy;
        // SetStatusText(character.electricAttack);
    }
}
