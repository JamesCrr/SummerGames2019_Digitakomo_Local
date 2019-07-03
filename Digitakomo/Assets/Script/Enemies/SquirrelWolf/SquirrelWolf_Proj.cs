using UnityEngine;

public class SquirrelWolf_Proj : MonoBehaviour
{
    [SerializeField]
    float MinDamage = 20f;
    [SerializeField]
    float MaxDamage = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProj")
            return;

        // If collide with playerProjectiles
        if (collision.gameObject.tag == "PlayerProj")
        {
            // If we collide with fire projectile, then destroy 
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.at == AttackType.FIRE)
            {
                gameObject.SetActive(false);
            }
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
        }
        gameObject.SetActive(false);
    }
}
