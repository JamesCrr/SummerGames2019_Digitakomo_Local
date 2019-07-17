using UnityEngine;

public class PT2Firewall : MonoBehaviour
{
    [Header("Creation Animation")]
    [SerializeField]
    Vector3 startingSize = Vector3.zero;
    [SerializeField]
    Vector3 endingSize = Vector3.one;
    [SerializeField]
    float growTime = 1.0f;
    float growTimer = 0.0f;

    [SerializeField]
    float MinDamage = 15f;
    [SerializeField]
    float MaxDamage = 20f;


    private void Start()
    {
        growTime = 1.0f / growTime;
    }

    void Update()
    {
        if (gameObject.transform.localScale != endingSize)
        {
            gameObject.transform.localScale = Vector3.Lerp(startingSize, endingSize, (growTimer * growTime));
            growTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProj")
        {
            // if hit by fire projectile, do nothing
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.GetMainType() == AttackType.FIRE)
                return;

            gameObject.SetActive(false);
            ScoreCalculator.Instance.AddScore(ScoreCalculator.SCORE_TYPE.PT2_FIRE);
        }
        // hit player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
            //gameObject.SetActive(false);
        }
    }

    public void ResetFlame()
    {
        gameObject.transform.localScale = startingSize;
        growTimer = 0.0f;
    }
}
