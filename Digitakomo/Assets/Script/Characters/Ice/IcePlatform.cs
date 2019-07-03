using UnityEngine;

public class IcePlatform : Weapon
{
    [SerializeField]
    LayerMask whatIsGround;
    private SpriteRenderer render;
    private Collider2D _collider;

    private float createdTime;
    private float preventFallTime = 0f;

    public float DropSpeed = 0.2f;
    Rigidbody2D ice_rb;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        ice_rb = GetComponent<Rigidbody2D>();
        ice_rb.isKinematic = true;
        render.enabled = false;
        _collider.enabled = false;
        createdTime = Time.time;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Time.time < createdTime + preventFallTime)
        {
            return;
        }
        if (collision.gameObject.GetComponent<IceCharacter>() != null)
        {
            ice_rb.isKinematic = false;
            ice_rb.gravityScale = DropSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            render.enabled = false;
            _collider.enabled = false;
            ice_rb.gravityScale = 0;
        }
    }

    public void Restart(Vector3 position)
    {
        ice_rb.gravityScale = 0;
        ice_rb.velocity = new Vector3(0, 0, 0);
        transform.position = position;
        render.enabled = true;
        _collider.enabled = true;
        ice_rb.isKinematic = true;
        createdTime = Time.time;
    }
}
