using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingVolumeManager : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider GameSlider;
    public Slider MusicSlider;

    private float MasterVolume = 1f;
    private float GameVolume = 1f;
    private float MusicVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // load from PlayerPref
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        GameSlider.value = PlayerPrefs.GetFloat("GameVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void OnValueChange(string name)
    {
        switch (name)
        {
            case "MasterVolume": ChangeValue(MasterSlider, name, SoundType.MASTER); break;
            case "GameVolume": ChangeValue(GameSlider, name, SoundType.GAME); break;
            case "MusicVolume": ChangeValue(MusicSlider, name, SoundType.MUSIC); break;
            default:
                Debug.LogWarning("SettingVolumeManager: No key found"); break;
        }
    }

    private void ChangeValue(Slider slider, string name, SoundType type)
    {
        PlayerPrefs.SetFloat(name, slider.value);
        // setsound
        if (type == SoundType.MASTER)
        {
            SoundManager.instance.SetMasterVolume(slider.value);
        }
        else
        {
            SoundManager.instance.SetVolumeBySoundType(type, slider.value);
        }
    }

}
