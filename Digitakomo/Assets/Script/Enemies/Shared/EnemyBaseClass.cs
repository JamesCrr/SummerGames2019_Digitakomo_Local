using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    // Enum for direction
    public enum DIRECTION
    {
        D_LEFT,
        D_RIGHT,
    }
    protected DIRECTION facingDirection = DIRECTION.D_RIGHT;


    // Stats
    [Header("EnemyBase Class")]
    [SerializeField]
    protected float hp = 1;
    protected float originalHp = 1;
    [SerializeField]
    protected int damage = 1;
    [SerializeField]
    protected float moveSpeed = 1.0f;
    // Attack
    [SerializeField]
    protected float attackTime = 2.0f;
    protected float attackTimer = 0.0f;

    // Spawn Zone
    protected SpawnZone spawningZone = null;

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;
    protected Animator myAnimator = null; // Animator
    protected Vector2 moveTargetPos = Vector2.zero;
    protected Vector2 moveDirection = Vector2.zero;


    // Init Function for general components
    protected void Init()
    {
        originalHp = hp;

        myRb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }


    // Called after being Fetched
    public virtual void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        // reset Hp
        hp = originalHp;
        // Attach new spawn zone
        spawningZone = newSpawnZone;
        // Reset Position
        myRb2D.position = newPos;
        transform.position = newPos;
    }

    // Called to move 
    protected virtual void Move()
    {
        moveDirection = moveTargetPos - myRb2D.position;
        // Flip the enemy?
        FlipEnemy();

        // Move
        myRb2D.MovePosition(myRb2D.position + (moveDirection.normalized * moveSpeed * Time.deltaTime));
    }
    // Called to stop all velocities of the RigidBody2D
    protected virtual void StopVel()
    {
        myRb2D.velocity = Vector2.zero;
        myRb2D.angularVelocity = 0.0f;
    }
    // Checks if you are have reached your target
    protected bool ReachedTarget(float magnitudeCheck = 1.0f)
    {
        if ((myRb2D.position - moveTargetPos).sqrMagnitude < magnitudeCheck)
            return true;

        return false;
    }
    // Called to Flip Sprite
    protected virtual void FlipEnemy()
    {
        // Do we need to switch direction?
        if ((facingDirection == DIRECTION.D_LEFT && moveDirection.x > -0.1f) ||
            (facingDirection == DIRECTION.D_RIGHT && moveDirection.x < 0.0f))
        {
            switch (facingDirection)
            {
                case DIRECTION.D_LEFT:
                    facingDirection = DIRECTION.D_RIGHT;
                    // Reverse the Object
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case DIRECTION.D_RIGHT:
                    facingDirection = DIRECTION.D_LEFT;
                    // Reverse the Object
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
            }
        }
    }


    // Called to modify the HP
    // Can be decrease or Increase
    // When HP reaches 0, gameObject is deactivated
    protected virtual void ModifyHealth(float modifyAmt)
    {
        hp += modifyAmt;
        if (hp < 1)
            gameObject.SetActive(false);
    }
}
