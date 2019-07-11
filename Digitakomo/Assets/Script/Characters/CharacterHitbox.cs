using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    public Character character;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ProjWithGround") && other.gameObject.tag == "Enemy")
        {
            // Hit by enemy
            EnemyBaseClass enemy = other.gameObject.GetComponentInParent<EnemyBaseClass>();
            if (enemy == null)
            {
                return;
            }
            EnemyAttackType enemyat = enemy.GetCurrentAttackType();
            switch (enemyat)
            {
                case EnemyAttackType.DRAGONSTALET: character.TakeDamage(enemy.damage); break;
                case EnemyAttackType.BLOODYSHINGLER_NORMAL: character.TakeDamage(enemy.damage); break;
                case EnemyAttackType.BLOODYSHINGLER_DASH: character.TakeDamage(enemy.GetComponent<PT2Script>().dartDamage); break;
                case EnemyAttackType.APES_NORMAL: character.TakeDamage(enemy.damage); break;
                case EnemyAttackType.APES_DASH: character.TakeDamage(enemy.GetComponent<AWScript>().Dash); break;
                case EnemyAttackType.APES_ATTACK: character.TakeDamage(enemy.GetComponent<AWScript>().Attack); break;
                case EnemyAttackType.APES_ROCK: character.TakeDamage(enemy.GetComponent<AWScript>().Rock); break;
                case EnemyAttackType.APES_SHAKE: character.TakeDamage(enemy.GetComponent<AWScript>().Shake); break;
            }
        }
    }
}
