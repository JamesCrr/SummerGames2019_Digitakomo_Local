using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWProj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProj")
            return;

        // If collide with playerProjectiles
        if (collision.gameObject.tag == "PlayerProj")
        {
            // If we collide with Ice projectile, then Freeze and drop 
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.at == AttackType.ICE)
            {
                gameObject.SetActive(false);
            }
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(1);
        }
        gameObject.SetActive(false);
    }
}
