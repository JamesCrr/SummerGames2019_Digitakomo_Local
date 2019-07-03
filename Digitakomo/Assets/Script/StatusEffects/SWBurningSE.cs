using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWBurningSE : BaseStatusEffect
{
    [Header("SWBurningSE")]
    [SerializeField]
    int damageToDeal = 1;
    [SerializeField]    // The Damaging Intervals
    float damageTime = 1.0f;
    float damageTimer = 0.0f;


    public override void onApply(EnemyBaseClass newEnemy)
    {
        base.onApply(newEnemy);

        // Set Position
        transform.position = enemyClass.GetFeetPosition();
        // Reset Damage Timer
        ResetDamageTimer();
    }

    public override void onLeave()
    {
        base.onLeave();
    }

    public override void onReApply()
    {
        base.onReApply();
    }

    public override bool UpdateEffect()
    {
        // if we just ended, then return
        if (!base.UpdateEffect())
            return false;

        // Set Position
        transform.position = enemyClass.GetFeetPosition();
        // Decrement Damage Timer
        damageTimer -= Time.deltaTime;
        if (damageTimer < 0.0f)
        {
            // Damage Enemy
            enemyClass.ModifyHealth(-damageToDeal);
            // Reset Timer
            ResetDamageTimer();
        }

        return true;
    }


    // Reset Timer
    void ResetDamageTimer()
    {
        damageTimer = damageTime;
    }
}
