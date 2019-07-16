using UnityEngine;

public class PixieType1PuddleScript : MonoBehaviour
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
            if (weapon.at == AttackType.FIRE)
            {
                gameObject.SetActive(false);
                ScoreCalculator.Instance.AddScore(ScoreCalculator.SCORE_TYPE.PT1_POISON);
            }
        }
        // hit player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
            //gameObject.SetActive(false);
        }
    }
}
