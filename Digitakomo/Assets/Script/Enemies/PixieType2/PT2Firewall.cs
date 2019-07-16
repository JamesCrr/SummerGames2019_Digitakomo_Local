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
}
