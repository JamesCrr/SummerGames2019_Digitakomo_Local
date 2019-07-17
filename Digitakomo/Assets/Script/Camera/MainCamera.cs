using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject[] characters;
    public Vector3 characterOffset;
    public float smoothing = 0.2f;

    public bool bounds;
    public Vector3 cameraLimit;
    public Vector3 cameraLimitCenter;

    // Start is called before the first frame update
    void Awake()
    {
        // Get All character
        characters = GameObject.FindGameObjectsWithTag("Player");
    }


    private void FixedUpdate()
    {
        float orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, CalculateCameraSize(characters), Time.deltaTime / smoothing);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, CalculateCameraPosition(characters), Time.deltaTime / smoothing);

        if (bounds)
        {
            // actual width and height of camera
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            // camera box

            // transform.position.y +- height; 
            // transform.position.x +- width;
            // Min X camera
            float MinCameraX = smoothedPosition.x - (width / 2);
            // Min X block
            float MinOffsetX = ((cameraLimitCenter.x - cameraLimit.x) / 2);
            float MaxCameraX = smoothedPosition.x + (width / 2);
            float MaxOffsetX = ((cameraLimitCenter.x + cameraLimit.x) / 2);
            // Check if lap
            if (MinCameraX <= MinOffsetX)
            {
                // use old position and size
                orthographicSize = Camera.main.orthographicSize;
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, smoothedPosition.y, smoothedPosition.z), 1);
            }

            // repeat but check for maximum
            else if (MaxCameraX >= MaxOffsetX)
            {
                orthographicSize = Camera.main.orthographicSize;
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, smoothedPosition.y, smoothedPosition.z), 1);
            }

            // Max Y position
            float MaxCameraY = smoothedPosition.y + (height / 2);
            float MaxOffsetY = ((cameraLimitCenter.y + cameraLimit.y) / 2);
            // Min Y positions
            float MinCameraY = smoothedPosition.y - (height / 2);
            float MinOffsetY = ((cameraLimitCenter.y - cameraLimit.y) / 2);
            if (MaxCameraY >= MaxOffsetY)
            {
                orthographicSize = Camera.main.orthographicSize;
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z), 1);
            }
            else if (MinCameraY <= MinOffsetY)
            {
                orthographicSize = Camera.main.orthographicSize;
                smoothedPosition = Vector3.Lerp(transform.position, new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z), 1);
            }
        }
        Camera.main.orthographicSize = orthographicSize;
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
            if (!character.activeSelf)
            {
                continue;
            }
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

        float minY = float.MaxValue;
        float maxY = float.MinValue;

        int charactercount = 0;
        foreach (GameObject character in characters)
        {
            if (!character.activeSelf)
            {
                continue;
            }
            charactercount++;
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
            if (minY > leftbotOffset.y)
            {
                minY = leftbotOffset.y;
            }
            if (maxY < righttopOffset.y)
            {
                maxY = righttopOffset.y;
            }
        }

        if (charactercount == 0)
        {
            return 10;
        }

        float width = maxX - minX;
        float height = maxY - minY;

        float targetRadio = width / height;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        if (screenRatio >= targetRadio)
        {
            float size = (height / charactercount);
            return size;
        }
        else
        {
            float size = width * Screen.height / Screen.width / charactercount;
            return size;
        }

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

        Gizmos.DrawWireCube(cameraLimitCenter, cameraLimit);
    }
}
