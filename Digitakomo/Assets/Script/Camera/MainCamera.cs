using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject[] characters;
    public float cameraOffset = 10f;
    // Start is called before the first frame update
    void Start()
    {
        // Get All character
        characters = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
