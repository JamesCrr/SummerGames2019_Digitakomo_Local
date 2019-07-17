using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;  // Used to access GameManager's Data

    public int PlayerCount = 2;
    public int score = 0;

    public IceCharacter IceCharacterObject;
    public FireCharacter FireCharacterObject;
    public Egg stone;
    private bool IsIceCharacter;

    [Header("This should be initialize already on MainMenu")]
    [Header("In case you're in development scene")]
    public GameObject ManagersPrefab;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCount = GameObject.FindGameObjectWithTag("Manager").GetComponent<CrossScene>().PlayerCount;
        // initialize the managers if on dev
        if (InputManager.Instance == null)
        {
            Instantiate(ManagersPrefab);
        }

        // if multiplayer active both of character
        if (PlayerCount == 2)
        {
            IceCharacterObject.gameObject.SetActive(true);
            FireCharacterObject.gameObject.SetActive(true);
        }
        // if single player active one of character randomly
        else if (PlayerCount == 1)
        {
            IceCharacterObject.gameObject.SetActive(false);
            FireCharacterObject.gameObject.SetActive(false);

            IceCharacterObject.player = 1;
            FireCharacterObject.player = 1;
            int randomedCharacter = Random.Range(0, 2);
            if (randomedCharacter == 0)
            {
                IceCharacterObject.gameObject.SetActive(true);
                IsIceCharacter = true;
            }
            else
            {
                FireCharacterObject.gameObject.SetActive(true);
                IsIceCharacter = false;
            }
        }
    }

    private void Update()
    {
        if (PlayerCount == 1)
        {
            // automatic change when one of the character die.
            if (IsIceCharacter)
            {
                if (IceCharacterObject.GetCurrentHP() <= 0)
                {
                    ChangeCharacter();
                }
            }
            else
            {
                if (FireCharacterObject.GetCurrentHP() <= 0)
                {
                    ChangeCharacter();
                }
            }
            // change the character manually
            if (InputManager.GetButtonDown("Player1ChangeCharacter"))
            {
                ChangeCharacter();
            }
        }

        if (IceCharacterObject == null)
        {
            return;
        }

        if (IceCharacterObject.GetCurrentHP() <= 0)
        {
            IceCharacterObject.gameObject.SetActive(false);
        }

        if (FireCharacterObject.GetCurrentHP() <= 0)
        {
            FireCharacterObject.gameObject.SetActive(false);
            SoundManager.instance.StopSound("Flamethrower");
        }

        if ((IceCharacterObject.GetCurrentHP() <= 0 && FireCharacterObject.GetCurrentHP() <= 0) || stone.GetCurrentHP() <= 0)
        {
            SoundManager.instance.StopAllSound();
            // Game End;
            SceneManager.LoadScene("SaveTest");
        }
    }

    private void ChangeCharacter()
    {
        if (IsIceCharacter)
        {
            FireCharacterObject.transform.position = IceCharacterObject.transform.position;
            IceCharacterObject.gameObject.SetActive(false);
            FireCharacterObject.gameObject.SetActive(true);
        }
        else
        {
            IceCharacterObject.transform.position = FireCharacterObject.transform.position;
            FireCharacterObject.gameObject.SetActive(false);
            IceCharacterObject.gameObject.SetActive(true);
        }
        IsIceCharacter = !IsIceCharacter;
    }
}
