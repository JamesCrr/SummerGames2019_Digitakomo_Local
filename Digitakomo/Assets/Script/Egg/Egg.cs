using UnityEngine;

public class Egg : MonoBehaviour, IDamagable
{
    // animation stuff
    protected enum Damaged
    {
        Normal,
        Level1,
        Level2,
        Level3,
        Level4
    }
    protected Damaged damaged = Damaged.Normal;
    protected Damaged oldState = Damaged.Normal;

    public Sprite Normal;
    public Sprite Level1;
    public Sprite Level2;
    public Sprite Level3;
    public Sprite Level4;

    // Instance
    public static Egg Instance = null;
    // Stats
    public float MaxHP = 5000;
    private float HP;
    // Other Components
    EggCenterPoint centerPoint = null;

    // 
    public int WarnPercentage = 30;

    // Awake
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // Set the stats
        HP = MaxHP;

        // Get Components
        centerPoint = GetComponent<EggCenterPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MaxHP * WarnPercentage / 100 >= HP)
        {
            // Debug.Log("Warning EGG HP LESS THAN " + WarnPercentage + " Percent");
        }

        HandleStateChanged();
    }

    private void HandleStateChanged()
    {
        if (oldState != damaged)
        {
            switch (damaged)
            {
                case Damaged.Normal: GetComponentInChildren<SpriteRenderer>().sprite = Normal; break;
                case Damaged.Level1: GetComponentInChildren<SpriteRenderer>().sprite = Level1; break;
                case Damaged.Level2: GetComponentInChildren<SpriteRenderer>().sprite = Level2; break;
                case Damaged.Level3: GetComponentInChildren<SpriteRenderer>().sprite = Level3; break;
                case Damaged.Level4: GetComponentInChildren<SpriteRenderer>().sprite = Level4; break;
            }
            oldState = damaged;
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (GetHPPercentage() <= 50)
        {
            damaged = Damaged.Level1;
        }
        if (GetHPPercentage() <= 30)
        {
            damaged = Damaged.Level2;
        }
        if (GetHPPercentage() <= 15)
        {
            damaged = Damaged.Level3;
        }
        if (GetHPPercentage() <= 1)
        {
            damaged = Damaged.Level4;
        }
    }

    public float GetHPPercentage()
    {
        return HP / MaxHP * 100;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public float GetCurrentHP()
    {
        return HP;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("ProjWithGround") && other.gameObject.tag == "Enemy")
        {
            // Hit by enemy
            EnemyBaseClass enemy = other.gameObject.GetComponentInParent<EnemyBaseClass>();
            EnemyAttackType enemyat = enemy.GetCurrentAttackType();
            switch (enemyat)
            {
                case EnemyAttackType.DRAGONSTALET: TakeDamage(enemy.damage); break;
                case EnemyAttackType.BLOODYSHINGLER_NORMAL: TakeDamage(enemy.damage); break;
                case EnemyAttackType.BLOODYSHINGLER_DASH: TakeDamage(enemy.GetComponent<PT2Script>().dartDamage); break;
                case EnemyAttackType.APES_NORMAL: TakeDamage(enemy.damage); break;
                case EnemyAttackType.APES_DASH: TakeDamage(enemy.GetComponent<AWScript>().Dash); break;
                case EnemyAttackType.APES_ATTACK: TakeDamage(enemy.GetComponent<AWScript>().Attack); break;
                case EnemyAttackType.APES_ROCK: TakeDamage(enemy.GetComponent<AWScript>().Rock); break;
                case EnemyAttackType.APES_SHAKE: TakeDamage(enemy.GetComponent<AWScript>().Shake); break;
            }
        }
    }


    #region Other Components
    public float GetStartingRadius()
    {
        return centerPoint.GetMaxSizeRadius();
    }
    #endregion

}
