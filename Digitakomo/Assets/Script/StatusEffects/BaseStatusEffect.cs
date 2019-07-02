using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatusEffect : MonoBehaviour
{
    [SerializeField]
    float effectDuration = 1.0f;
    float effectTimer = 0.0f;
    protected EnemyBaseClass enemyClass = null;


    public virtual void onApply(EnemyBaseClass newEnemy)
    {
        enemyClass = newEnemy;
        effectTimer = effectDuration;
    }

    public virtual bool UpdateEffect()      // Count down Timer
    {
        if (effectTimer < 0.0f)
            return false;

        effectTimer -= Time.deltaTime;

        return true;
    }

    public virtual void onLeave()   // Reset Timer
    {
        effectTimer = effectDuration;
    }

    public virtual void onReApply()  // Reset Timer
    {
        effectTimer = effectDuration;
    }
}
