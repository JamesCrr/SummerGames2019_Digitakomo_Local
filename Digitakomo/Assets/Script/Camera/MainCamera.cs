using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject[] characters;
    public Vector3 characterOffset;
    public float smoothing = 0.2f;
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

    private void FixedUpdate()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, CalculateCameraSize(characters), Time.deltaTime / smoothing);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, CalculateCameraPosition(characters), Time.deltaTime / smoothing);
        transform.position = smoothedPosition;
    }

    Vector3 CalculateCameraPosition(GameObject[] characters)
    {
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        float minX = float.MaxValue;
        float minY = float.MaxValue;

        foreach (GameObject character in characters)
        {
            Vector3 leftbotOffset = character.transform.position - (characterOffset / 2);
            Vector3 righttopOffset = character.transform.position + (characterOffset / 2);

            if (maxY < righttopOffset.y)
            {
                maxY = righttopOffset.y;
            }
            if (maxX < righttopOffset.x)
            {
                maxX = righttopOffset.x;
            }
            if (minX > leftbotOffset.x)
            {
                minX = leftbotOffset.x;
            }
            if (minY > leftbotOffset.y)
            {
                minY = leftbotOffset.y;
            }
        }


        //findY
        float positionY = (minY + maxY) * 0.5f;
        //findX
        float positionX = (minX + maxX) * 0.5f;

        // Z doesn't change according to 2d
        return new Vector3(positionX, positionY, transform.position.z);
    }

    float CalculateCameraSize(GameObject[] characters)
    {
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
        }

        float width = maxX - minX;

        float size = width * Screen.height / Screen.width * 0.5f;

        return size;
    }

    private void OnDrawGizmos()
    {
        if (characters == null)
        {
            return;
        }
        Gizmos.color = new Color(0, 0, 0, 0.1f);

        foreach (GameObject character in characters)
        {
            Gizmos.DrawWireCube(character.transform.position, characterOffset);
        }
    }
}
