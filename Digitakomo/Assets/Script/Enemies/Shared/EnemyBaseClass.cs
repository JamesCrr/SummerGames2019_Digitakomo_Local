﻿using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    [System.Serializable]   // Used for platform detection
    public class DetectBox
    {
        public Vector2 detectOffset;
        public Vector2 detectSize;
    }
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
    protected int hp = 1;
    protected int originalHp = 1;
    [SerializeField]
    protected int damage = 1;
    [SerializeField]
    protected int defense = 0;
    [SerializeField]
    protected float moveSpeed = 1.0f;
    // Attack
    [SerializeField]
    protected float attackTime = 2.0f;
    protected float attackTimer = 0.0f;
    // Status Effects
    protected StatusEffectManager seManager = null;

    // Spawn Zone
    protected SpawnZone spawningZone = null;

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;
    protected Animator myAnimator = null; // Animator
    protected Vector2 moveTargetPos = Vector2.zero;
    protected Vector2 moveDirection = Vector2.zero;
    [SerializeField]
    Transform feetPosition = null;


    // Init Function for general components
    protected void Init()
    {
        originalHp = hp;

        myRb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        seManager = new StatusEffectManager(this.gameObject);
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
    // Can be Decrease or Increase
    // When HP reaches 0, gameObject is deactivated
    public virtual void ModifyHealth(int modifyAmt)
    {
        // if we are taking damage, then reduce damage with defense
        if (modifyAmt < 0)
            modifyAmt += defense;

        hp += modifyAmt;
        if (hp < 1)
        {
            // damage text
            FloatingTextController.CreateFloatingText("0", transform.position);
            // Remove all status effects
            seManager.RemoveAllStatusEffects();
            gameObject.SetActive(false);
        }
        else
        {
            // damage text
            FloatingTextController.CreateFloatingText(hp.ToString(), transform.position);
        }
    }
    // Returns Hp
    public float GetCurrentHP()
    {
        return hp;
    }
    // Returns Bool if enemy has died
    public bool IsDead()
    {
        if (hp <= 0)
            return true;
        return false;
    }

    // Modify the Animator's Speed
    public void SetAnimatorSpeed(float newSpeed)
    {
        myAnimator.speed = newSpeed;
    }

    // Returns the Feet Position
    public Vector3 GetFeetPosition()
    {
        return feetPosition.position;
    }
}
