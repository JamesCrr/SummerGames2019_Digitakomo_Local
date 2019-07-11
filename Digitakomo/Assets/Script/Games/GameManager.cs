using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int PlayerCount = 2;
    public int score = 0;

    public IceCharacter IceCharacterObject;
    public FireCharacter FireCharacterObject;
    private bool IsIceCharacter;

    [Header("This should be initialize already on MainMenu")]
    [Header("In case you're in development scene")]
    public GameObject ManagersPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCount = GameObject.FindGameObjectWithTag("Manager").GetComponent<CrossScene>().PlayerCount;
        // initialize the managers if on dev
        if (InputManager.Instance == null)
        {
            Instantiate(ManagersPrefab);
        }

        if (PlayerCount == 2)
        {
            IceCharacterObject.gameObject.SetActive(true);
            FireCharacterObject.gameObject.SetActive(true);
        }
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
        if (PlayerCount == 1 && InputManager.GetButtonDown("Player1ChangeCharacter"))
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
}
