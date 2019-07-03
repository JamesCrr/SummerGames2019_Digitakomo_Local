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

    public virtual void onLeave()   // Reset Timer and Disable GameObject
    {
        effectTimer = effectDuration;
        gameObject.SetActive(false);
    }

    public virtual void onReApply()  // Reset Timer
    {
        effectTimer = effectDuration;
    }

    public void SetToDone()     // Sets the Timer to -1, so won't update anymore
    {
        effectTimer = -1.0f;
    }
}
