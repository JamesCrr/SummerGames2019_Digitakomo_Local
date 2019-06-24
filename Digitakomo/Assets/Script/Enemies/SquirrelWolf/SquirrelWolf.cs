﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    // States
    enum STATES
    {
        S_WALKTOEGG,
        S_WALKTOPLAYER,
        S_WALKBACK,
        S_MELEE,
        S_SHOOT,
    }
    STATES currentState;
    [Header("SquirrelWolf Class")]
    [SerializeField]
    // How far to detect for player
    float playerDetectionRange = 0.0f;
    Collider2D result = null;
    [SerializeField]
    // How far before we use melee to attack
    float minimumMeleeRange = 4.0f;
    [SerializeField]
    // Minimum attacking Distance for melee
    float meleeAttackDistance = 1.0f;
    [Header("Shooting")]
    // What to shoot
    [SerializeField]
    GameObject projectilePrefab = null;
    [SerializeField]
    Transform shootingPos = null;
    [SerializeField]
    // Maximum Attack Range
    float maxShootingRange = 5.0f;
    [SerializeField]
    float timeToHitTarget = 1.0f;   // How long for the projectile to hit smth


    [SerializeField]
    // Ground Cast
    Transform groundCast = null;
    [SerializeField]
    float groundCastLength = 2.0f;


    // Unity Stuff
    // Get referrence to the target Object
    GameObject targetObject = null;


    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();
        currentState = STATES.S_WALKTOEGG;

        // Convert int to floats for easier calculation
        timeToHitTarget = 1 / timeToHitTarget;
    }

    // Update is called once per frame
    void Update()
    {
        // Update State Machine
        UpdateStates();
    }


    // Used to define this AI's logic
    void UpdateStates()
    {

        Debug.Log(groundCast.position);

        switch (currentState)
        {
            case STATES.S_WALKTOEGG:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if(targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_WALKTOPLAYER;
                        return;
                    }
                    // set the position of egg and move there
                    targetObject = Egg.Instance.gameObject;


                    // check if we can shoot projectile at egg
                    if (((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude <= (maxShootingRange * maxShootingRange))
                    {
                        currentState = STATES.S_SHOOT;
                        return;
                    }
                    // Move enemy
                    if (!MoveWolf())
                    {
                        // Walk back
                        currentState = STATES.S_WALKBACK;
                        TurnWolfAround();
                        return;
                    }

                    // Check if we can attack using melee
                    if (((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude < meleeAttackDistance)
                        currentState = STATES.S_MELEE;
                }
                break;
            case STATES.S_WALKTOPLAYER:
                {
                    // check if we can ATTACK player 
                }
                break;
            case STATES.S_WALKBACK:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if (targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_WALKTOPLAYER;
                        return;
                    }

                    // Move enemy
                    if (!MoveWolf())
                    {
                        TurnWolfAround();
                        return;
                    }
                }
                break;
            case STATES.S_MELEE:
                {

                }
                break;
            case STATES.S_SHOOT:
                {
                    attackTimer -= Time.deltaTime;
                    if(attackTimer < 0.0f)
                    {
                        Shoot();
                        // Reset Timer
                        attackTimer = attackTime;

                        // Is a player in range?
                        targetObject = IsPlayerInRange();
                        if (targetObject != null)    // Found Player
                        {
                            currentState = STATES.S_WALKTOPLAYER;
                            return;
                        }
                    }
                }
                break;
        }
    }

    // Returns the player Object if he is in Range
    // Does not count EGG as player
    GameObject IsPlayerInRange()
    {
        // Get if we have hit anything, player or egg
        result = Physics2D.OverlapCircle(myRb2D.position, playerDetectionRange, LayerMask.NameToLayer("Player"));
        if (result == null)
            return null;
        if (result.gameObject.tag != "Player")
            return null;

        return result.gameObject;
    }
    // Custom Move Function as we need to return a value
    // Returns false when can no longer move
    // Returns true if can still move
    private bool MoveWolf()
    {
        // set the new direction
        direction = targetPosition - myRb2D.position;
        direction.y = 0.0f;
        direction.Normalize();

        // Check if we can even move
        if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength)))
        {
            // Move
            myRb2D.MovePosition(myRb2D.position + (direction * moveSpeed * Time.deltaTime));

            return true;
        }
        return false;
    }
    // Used to turn the wolf around
    private void TurnWolfAround()
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
        
        // set new target
        direction = -direction;
        targetPosition.Set(myRb2D.position.x + (direction.x * 100.0f), targetPosition.y);
        // turn the ground cast pos 

    }
    // Shooting Logic
    private void Shoot()
    {
        GameObject newProj = ObjectPooler.Instance.FetchGO_Pos(projectilePrefab.name, shootingPos.position);

        Vector2 launchVelocity = Vector2.zero;
        launchVelocity.x = (targetObject.transform.position.x - shootingPos.position.x) * timeToHitTarget;    // Initial velocity in X axis
        launchVelocity.y = -(-(targetObject.transform.position.y - shootingPos.position.y) + 0.5f * Physics2D.gravity.y * timeToHitTarget * timeToHitTarget) * timeToHitTarget;
        //launchVelocity.Set(Mathf.Cos(45.0f) * projectileSpeed, Mathf.Sin(45.0f) * projectileSpeed);

        // Add the velocity to the object
        newProj.GetComponent<Rigidbody2D>().velocity = launchVelocity;
    }


    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundCast.position, groundCast.position + (Vector3.down * groundCastLength));
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxShootingRange);
    }
}