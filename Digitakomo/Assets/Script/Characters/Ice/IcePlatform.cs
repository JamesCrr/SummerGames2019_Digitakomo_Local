using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsGround;
    private SpriteRenderer render;
    private Collider2D _collider;

    public float DropSpeed = 0.2f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        render.enabled = false;
        _collider.enabled = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IceCharacter>() != null)
        {
            rb.gravityScale = DropSpeed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.Log(whatIsGround.value, 2))
        {
            render.enabled = false;
            _collider.enabled = false;
            rb.gravityScale = 0;
        }
    }

    public void Restart(Vector3 position)
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = position;
        render.enabled = true;
        _collider.enabled = true;
    }
}
