
using UnityEngine;

public class PixieType1ProjectileScript : MonoBehaviour
{
    [Header("Fall Speed")]
    [SerializeField]
    float fallSpeed = 1.0f;

    // Unity Stuff
    Rigidbody2D myRb2D;


    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fall
        myRb2D.MovePosition(myRb2D.position + (Vector2.down * fallSpeed * Time.deltaTime));
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy")
            return;

        gameObject.SetActive(false);
    }
}
