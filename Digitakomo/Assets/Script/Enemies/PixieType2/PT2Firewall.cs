using UnityEngine;

public class PT2Firewall : MonoBehaviour
{
    [SerializeField]
    float MinDamage = 15f;
    [SerializeField]
    float MaxDamage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProj")
        {
            // if hit by fire projectile
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            Debug.Log(weapon.at);
            if (weapon.at == AttackType.ICE)
            {
                gameObject.SetActive(false);
            }
        }
        // hit player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
            gameObject.SetActive(false);
        }
    }
}
