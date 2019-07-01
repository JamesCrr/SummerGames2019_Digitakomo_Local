using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject[] characters;
    public Vector3 characterOffset;
    // Start is called before the first frame update
    void Start()
    {
        // Get All character
        // characters = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateCameraPosition(GameObject[] characters)
    {

    }

    void CalculateCameraSize(GameObject[] characters)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0, 1f);

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (GameObject character in characters)
        {
            Vector3 leftbotOffset = character.transform.position - (characterOffset / 2);
            Vector3 righttopOffset = character.transform.position + (characterOffset / 2);

            if (minX > leftbotOffset.x)
            {
                minX = leftbotOffset.x;
            }
            if (maxX < righttopOffset.x)
            {
                maxX = righttopOffset.x;
            }

            Gizmos.DrawWireCube(character.transform.position, characterOffset);
        }

        Gizmos.DrawLine(new Vector3(minX, 0, 0), new Vector3(maxX, 0, 0));
    }
}
