using UnityEngine;

public enum SoundType
{
    GAME,
    MUSIC,
    MASTER
}

[System.Serializable]
public class Sound
{
    public string name;
    public SoundType type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]

    public float randomVolume = 0.1f;

    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public float masterVolume = 1f;
    public float settingVolume = 1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.loop = loop;
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)) * masterVolume * settingVolume;
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        // I don't know why it's not working
        source.Stop();
    }

    public void SetVolume(float _volume)
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)) * masterVolume * settingVolume;
        settingVolume = _volume;
    }

    public void SetMasterVolume(float _masterVolume)
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)) * _masterVolume * settingVolume;
        masterVolume = _masterVolume;
    }

}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager in the scene");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // if never set any volume
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            // Set volume by default
            PlayerPrefs.SetFloat("MasterVolume", 1.0f);
            PlayerPrefs.SetFloat("GameVolume", 1.0f);
            PlayerPrefs.SetFloat("MusicVolume", 1.0f);
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        foreach (Sound sound in sounds)
        {
            GameObject _go = new GameObject("Sound_" + sound.name);
            _go.transform.SetParent(this.transform);
            sound.SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("Audio Manager(Play): Sound not found in list, " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        Debug.LogWarning("Audio Manager(Stop): Sound not found in list, " + _name);
    }

    public void SetVolumeBySoundType(SoundType _type, float _volume)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == _type)
            {
                sound.SetVolume(_volume);
            }
        }
    }

    public void SetMasterVolume(float _volume)
    {
        foreach (Sound sound in sounds)
        {
            sound.SetMasterVolume(_volume);
        }
    }
}
