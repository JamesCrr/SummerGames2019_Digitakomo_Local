using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQFrozenSE : BaseStatusEffect
{
    [SerializeField]
    float slideFactor = 2.5f;
    // Is the ice currently moving?
    bool isMoving = false;

    
    public override void onApply(EnemyBaseClass newEnemy)
    {
        base.onApply(newEnemy);
        // Reactivate Collider for ownself
        GetComponent<Collider2D>().enabled = true;
        // Disable the Enemy's collider and rigidbody
        enemyClass.gameObject.GetComponent<Collider2D>().enabled = false;
        enemyClass.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        enemyClass.SetAnimatorSpeed(0.0f);
        // parent the enemy 
        enemyClass.gameObject.transform.parent = transform;

        // Class Specific
        if (newEnemy.gameObject.GetComponent<SquirrelWolf>())
            newEnemy.gameObject.GetComponent<SquirrelWolf>().SetFrozen(true);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If we are not moving yet, then wait for player collision
        if (!isMoving)
        {
            // collide with player?
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.tag != "Player")
                return;

            // Add velocity??
            Vector2 newVel = Vector2.zero;
            newVel = GetComponent<Rigidbody2D>().velocity;
            newVel.x *= slideFactor;
            GetComponent<Rigidbody2D>().velocity = newVel;

            isMoving = true;
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
        enemyClass.SetAnimatorSpeed(1.0f);

        // Class Specific
        if (enemyClass.gameObject.GetComponent<SquirrelWolf>())
            enemyClass.gameObject.GetComponent<SquirrelWolf>().SetFrozen(false);
    }
}
