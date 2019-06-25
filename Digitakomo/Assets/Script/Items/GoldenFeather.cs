using UnityEngine;

public class GoldenFeather : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colliderenter");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
    }
}
