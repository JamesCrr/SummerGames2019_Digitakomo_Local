using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWProj : MonoBehaviour
{
    bool frozen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProj")
            return;

        if (!frozen)
        {
            // If collide with playerProjectiles
            if (collision.gameObject.tag == "PlayerProj")
            {
                // If we collide with Ice projectile, then Freeze and drop 
                Weapon weapon = collision.gameObject.GetComponent<Weapon>();
                if (weapon.at == AttackType.ICE)
                {
                    //gameObject.SetActive(false);

                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().isKinematic = false;
                    frozen = true;
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<IDamagable>().TakeDamage(1);
                gameObject.SetActive(false);
            }
            return;
        }

        // if we hit the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.SetActive(false);
            frozen = false;
        }
    }
}
