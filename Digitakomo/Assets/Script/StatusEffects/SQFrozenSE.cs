using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQFrozenSE : BaseStatusEffect
{
    bool isMoving = false;

    public override void onApply(EnemyBaseClass newEnemy)
    {
        base.onApply(newEnemy);
        // Reactivate Collider for ownself
        GetComponent<Collider2D>().enabled = true;
        // Disable the Enemy's collider and rigidbody
        enemyClass.gameObject.GetComponent<Collider2D>().enabled = false;
        enemyClass.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        // parent the enemy 
        enemyClass.gameObject.transform.parent = transform;

        // Reset Status Effects Data
        isMoving = false;
    }

    public override void onLeave()
    {
        base.onLeave();
        // defrost
        DeFrost();
    }

    public override void onReApply()
    {

    }

    public override bool UpdateEffect()
    {
        // if we just ended, then return
        if (!base.UpdateEffect())
            return false;

        return true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are not moving yet, then wait for player collision
        if(!isMoving)
        {
            // collide with player?
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.tag != "Player")
                return;
            
            // Add velocity??
        }

        // Only accepts collision from other wolfs
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;
        SquirrelWolf otherWolf = collision.gameObject.GetComponent<SquirrelWolf>();
        if (otherWolf == null)
            return;

        // Damage other wolf
        otherWolf.ModifyHealth(-1);
        // defrost
        DeFrost();
    }
    

    // Function for Defrosting logic
    void DeFrost()
    {
        // remove enemy from child list
        enemyClass.gameObject.transform.parent = null;
        // Deactive self
        GetComponent<Collider2D>().enabled = false;
        transform.gameObject.SetActive(false);
            
        // Reenable the Enemy's collider and rigidbody
        enemyClass.gameObject.GetComponent<Collider2D>().enabled = true;
        enemyClass.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
