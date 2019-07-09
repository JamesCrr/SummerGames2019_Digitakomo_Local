using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int PlayerCount = 2;
    public int score = 0;

    [Header("This should be initialize already on MainMenu")]
    [Header("In case you're in development scene")]
    public GameObject ManagersPrefab;
    public GameObject SoundsPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // initialize the player somehow
        if (InputManager.Instance == null)
        {
            Instantiate(ManagersPrefab);
        }
    }
}
