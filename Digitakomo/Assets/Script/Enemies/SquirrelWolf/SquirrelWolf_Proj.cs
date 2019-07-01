using UnityEngine;

public class SquirrelWolf_Proj : MonoBehaviour
{
    [SerializeField]
    float MinDamage = 20f;
    [SerializeField]
    float MaxDamage = 30f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProj")
            return;

        // If we collide with fire projectile, then destroy self

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
        }
        gameObject.SetActive(false);
    }
}
