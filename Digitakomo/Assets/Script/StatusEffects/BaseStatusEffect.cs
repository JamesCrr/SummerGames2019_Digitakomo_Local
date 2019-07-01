using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatusEffect : MonoBehaviour
{
    float effectTimer = 0.0f;
    float effectDuration = 1.0f;
    EnemyBaseClass enemyClass = null;


    protected virtual void onApply(EnemyBaseClass newEnemy)
    {

    }

    protected virtual void UpdateEffect()
    {
        if (effectTimer < 0.0f)
            return;

        effectTimer -= Time.deltaTime;
    }
    protected virtual void onLeave()
    {
        effectTimer = effectDuration;
    }


}
