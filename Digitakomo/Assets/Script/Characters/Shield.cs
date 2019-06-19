using UnityEngine;

public class Shield : MonoBehaviour
{
    private Collider2D _Shield;
    private SpriteRenderer Texture;
    private bool isShielding = false;
    private bool isShieldUp = false;
    private Quaternion DefaultRotation;

    Character player;

    void Awake()
    {
        _Shield = gameObject.GetComponentInChildren<Collider2D>();
        Texture = gameObject.GetComponentInChildren<SpriteRenderer>();
        player = gameObject.GetComponentInParent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.isLockMovement += 1;
            isShielding = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            player.isLockMovement -= 1;
            isShielding = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            isShieldUp = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isShieldUp = false;
        }
    }

    void FixedUpdate()
    {
        SetShieldEnable(isShielding);
        SetRotation(isShieldUp);
    }

    private void SetRotation(bool rotate)
    {
        if (rotate)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            transform.rotation = player.transform.rotation;
        }
    }

    private void SetShieldEnable(bool enable)
    {
        _Shield.enabled = enable;
        Texture.enabled = enable;
    }
}
