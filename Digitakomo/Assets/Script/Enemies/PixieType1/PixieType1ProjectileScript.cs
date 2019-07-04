
using UnityEngine;

public class PixieType1ProjectileScript : MonoBehaviour
{
    [Header("Fall Speed")]
    [SerializeField]
    float fallSpeed = 1.0f;
    [SerializeField]
    float MinDamage = 15f;
    [SerializeField]
    float MaxDamage = 20f;

    // Unity Stuff
    Rigidbody2D myRb2D;


    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fall
        myRb2D.MovePosition(myRb2D.position + (Vector2.down * fallSpeed * Time.deltaTime));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy")
            return;

        // Spawn new Puddle if hit groud
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            ObjectPooler.Instance.FetchGO_Pos("PT1_PuddleProj", transform.position);
        // hit player
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
        } 
        // hit player projectile
        else if(collision.gameObject.tag != "PlayerProj")
        {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            AttackType type = weapon.at;
            // if immune
            if (type == AttackType.ICE || type == AttackType.ICE_JUMP || type == AttackType.Normal)
                return;
        }

        // Despawn  
        gameObject.SetActive(false);
    }
}
