using UnityEngine;

public class Shield : MonoBehaviour, IDamagable
{
    private Collider2D _Shield;
    private SpriteRenderer Texture;
    private bool isShielding = false;

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
        // Debug.Log("update");
        if (InputManager.GetButtonDown("Player" + player.player + "Defense"))
        {
            Debug.Log("Defensing");
            player.isLockMovement += 1;
            isShielding = true;
            player.Animate.SetTrigger("preDefense");
            player.Animate.SetBool("isDefensing", true);
        }
        if (InputManager.GetButtonUp("Player" + player.player + "Defense"))
        {
            player.isLockMovement -= 1;
            isShielding = false;
            player.Animate.SetBool("isDefensing", false);
        }
        if (isShielding && InputManager.GetAxisRaw("Player" + player.player + "LookUp") == 1)
        {
            // Shield
            SetRotation(true);
        }
        else if (isShielding && InputManager.GetAxisRaw("Player" + player.player + "LookUp") == 0)
        {
            SetRotation(false);
        }
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

    //private void SetShieldEnable(bool enable)
    //{
    //    _Shield.enabled = enable;
    //    Texture.enabled = enable;
    //}

    public void TakeDamage(float damage)
    {
        // Not taking any damage
    }
}
