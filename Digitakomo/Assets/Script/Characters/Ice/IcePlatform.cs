using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    private SpriteRenderer render;
    private Collider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IceCharacter>() != null)
        {
            render.enabled = false;
            _collider.enabled = false;
        }
    }
}
