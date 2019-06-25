using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Range(0, 50)]
    public float MinDamage;
    public float MaxDamage;
    private float actualDamage;
    public float speed = 5.0f;
    public float timeoutDestructor = 1f;
    private float currentTime;
    private Rigidbody2D rb;
    public AttackType at = AttackType.UNKNOWN;
    public float nonAttackTypeDivider = 15;
    public float enerygyPerSpecialAttack = 1;
    public float firerate;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Restart();
    }

    protected virtual void FixedUpdate()
    {
        if (currentTime + timeoutDestructor <= Time.time)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMissileDirection(int x, int y)
    {
        rb.velocity = new Vector2(x * speed, y * speed);
    }

    public void SetRotation(int rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public float GetDamageWithAttackType(AttackType at)
    {
        if (this.at == at)
        {
            return actualDamage / nonAttackTypeDivider;
        }
        return actualDamage;
    }

    public float GetActualDamage()
    {
        return actualDamage;
    }

    public virtual void Restart()
    {
        currentTime = Time.time;
        actualDamage = Random.Range(MinDamage, MaxDamage);
    }
}
