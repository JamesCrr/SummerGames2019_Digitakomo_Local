using UnityEngine;

public class Shield : MonoBehaviour
{
    private Collider2D _Shield;
    private SpriteRenderer Texture;
    private bool isShielding = false;
    private bool isShieldUp = false;
    private Quaternion DefaultRotation;

    Character player;
    private int noPlayer;

    void Awake()
    {
        _Shield = gameObject.GetComponentInChildren<Collider2D>();
        Texture = gameObject.GetComponentInChildren<SpriteRenderer>();
        player = gameObject.GetComponentInParent<Character>();
        noPlayer = player.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Player" + noPlayer + "Defense"))
        {
            player.isLockMovement += 1;
            isShielding = true;
        }
        if (Input.GetButtonUp("Player" + noPlayer + "Defense"))
        {
            player.isLockMovement -= 1;
            isShielding = false;
        }
        if (Input.GetButtonDown("Player" + noPlayer + "LookUp"))
        {
            isShieldUp = true;
        }
        if (Input.GetButtonUp("Player" + noPlayer + "LookUp"))
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
