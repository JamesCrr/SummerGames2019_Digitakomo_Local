using UnityEngine;

public class RangeWeapon : Weapon
{
    public float speed = 5.0f;
    public float timeoutDestructor = 1f;
    private float currentTime;
    private Rigidbody2D rb;
    public float enerygyPerSpecialAttack = 1;
    public float firerate;

    void Start()
    {
        Restart();
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (currentTime + timeoutDestructor <= Time.time)
        {
            disableProjectile();
        }
    }

    protected virtual void disableProjectile()
    {
        gameObject.SetActive(false);
    }

    public void SetMissileDirection(int x, int y)
    {
        rb.velocity = new Vector2(x * speed, y * speed);
    }

    public void SetRotation(int rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public virtual void Restart()
    {
        currentTime = Time.time;
    }
}

