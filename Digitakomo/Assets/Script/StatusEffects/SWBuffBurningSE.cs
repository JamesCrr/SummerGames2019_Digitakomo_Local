using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWBuffBurningSE : BaseStatusEffect
{
    public override void onApply(EnemyBaseClass newEnemy)
    {
        base.onApply(newEnemy);

        // Set Position
        transform.position = enemyClass.GetFeetPosition();
        // Buff the enemy
        enemyClass.ModifyDefense(2);
    }

    public override void onLeave()
    {
        // Debuff the enemy
        enemyClass.SetDefense(0);
        base.onLeave();
    }

    public override void onReApply()
    {
        //base.onReApply();
    }

    public override bool UpdateEffect()
    {
        // if we just ended, then return
        if (!base.UpdateEffect())
            return false;

        // Set Position
        transform.position = enemyClass.GetFeetPosition();

        return true;
    }
}
