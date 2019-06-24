using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    // States
    enum STATES
    {
        S_EGG_SIMILARHEIGHT,
        S_EGG_DIFFERENTHEIGHT,
        S_FOUNDPLAYER,
        S_WALKBACK,
        S_RUNAWAY,

        S_JUMPING,
        S_MELEE,
        S_SHOOT,
    }
    STATES currentState;
    #region Data
    [Header("SquirrelWolf Class")]
    [SerializeField]
    // How far to detect for player
    float playerDetectionRange = 0.0f;
    Collider2D result = null;
    [Header("Jumping")]
    [SerializeField]
    // How far to detect for jumping platform 
    Vector2 platformDetectOffset = Vector2.zero;    // The Position Offset of the detecting box
    [SerializeField]
    Vector2 platformDetectSize = Vector2.zero;      // Size of detecting box
    List<Collider2D> listOfPlatforms = new List<Collider2D>();    // Used to store the platforms that we can jump to
    ContactFilter2D jumpingFilter = new ContactFilter2D();      // To prevent me calling new everytime
    bool isGrounded = false;     // Used to check if we have reached the ground
    [SerializeField]
    groundCheck groundCheckScript = null;   // Script used to check if have reached the ground when jumping

    [Header("Melee")]
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
    [Header("GroundCasting")]
    [SerializeField]
    // Ground Cast
    Transform groundCast = null;
    [SerializeField]
    float groundCastLength = 2.0f;
    [Header("RunAwayTimer")]
    [SerializeField]
    float fleeTime = 5.0f;
    float fleeTimer = 0.0f;
    // Unity Stuff
    // Get referrence to the target Object
    GameObject targetObject = null;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();
        currentState = STATES.S_EGG_DIFFERENTHEIGHT;

        // Convert int to floats for easier calculation
        timeToHitTarget = 1 / timeToHitTarget;
        // Set the Flee Timer
        fleeTimer = fleeTime;

        // Set the jumping Filters
        jumpingFilter.SetLayerMask(LayerMask.GetMask("JumpPointLayer"));
        jumpingFilter.ClearDepth();
        jumpingFilter.useTriggers = true;
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

        switch (currentState)
        {
            case STATES.S_EGG_SIMILARHEIGHT:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if(targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_FOUNDPLAYER;
                        return;
                    }
                    // Check if a change in state has been made
                    if (CheckHeightDifference())
                        return;


                    // set the position of egg and move there
                    targetObject = Egg.Instance.gameObject;
                    moveTargetPos = targetObject.transform.position;
                    // check if we can shoot projectile at egg
                    if ((moveTargetPos - myRb2D.position).sqrMagnitude <= (maxShootingRange * maxShootingRange))
                    {
                        currentState = STATES.S_SHOOT;
                        return;
                    }

                    // Move enemy
                    if (!MoveWolf())
                    {

                        // If we don't find any platforms to jump to, then we turn back
                        Vector2 platformEdgePos = FindNearestPlatform();
                        if (platformEdgePos == Vector2.zero)
                        {
                            // Walk back
                            //currentState = STATES.S_WALKBACK;
                            //TurnWolfAround();
                        }
                        else
                        {
                            // Jump State
                            currentState = STATES.S_JUMPING;
                            // Set new target and jump there
                            moveTargetPos = platformEdgePos;
                            JumpWolf(moveTargetPos);
                        }

                        return;
                    }

                    // Check if we can attack using melee
                    if ((moveTargetPos - myRb2D.position).sqrMagnitude < meleeAttackDistance)
                        currentState = STATES.S_MELEE;
                }
                break;
            case STATES.S_EGG_DIFFERENTHEIGHT:
                {
                    // Check if a change in state has been made
                    if (CheckHeightDifference())
                        return;
                    // If we are not grounded yet, return
                    if (!isGrounded)
                        return;
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if (targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_FOUNDPLAYER;
                        return;
                    }

                    // the closet point to the egg
                }
                break;
            case STATES.S_FOUNDPLAYER:
                {
                    // check if we can ATTACK player or need to walk there


                }
                break;
            case STATES.S_WALKBACK:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if (targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_FOUNDPLAYER;
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
            case STATES.S_RUNAWAY:
                {
                    // if grounded, just return
                    if (isGrounded)
                        return;

                    // Move enemy
                    if (!MoveWolf())
                    {
                        // If we don't find any platforms to jump to, then we turn back
                        Vector2 platformEdgePos = FindFurtherestPlatform();
                        if (platformEdgePos == Vector2.zero)
                        {
                            // Go back to finding statew
                            currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        }
                        else
                        {
                            // Jump State
                            currentState = STATES.S_JUMPING;
                            // Set new target and jump there
                            moveTargetPos = platformEdgePos;
                            JumpWolf(moveTargetPos);
                        }

                        return;
                    }

                    //fleeTimer -= Time.deltaTime;
                    //if(fleeTimer < 0.0f)
                    //{
                    //    currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                    //    fleeTimer = fleeTime;
                    //}
                }
                break;


            case STATES.S_JUMPING:
                {
                    // Check if we are reaching the target from jumping
                    if (isGrounded)
                        currentState = STATES.S_EGG_DIFFERENTHEIGHT;
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
                        // Shoot
                        Shoot();
                        // Reset Timer
                        attackTimer = attackTime;

                        // Is a player in range?
                        targetObject = IsPlayerInRange();
                        if (targetObject != null)    // Found Player
                            currentState = STATES.S_FOUNDPLAYER;
                        else
                            currentState = STATES.S_EGG_SIMILARHEIGHT;

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
    // Fills the list with platforms that are within my Collider
    // Returns Vector2.zero if no Colliders Found
    // Returns the Position if found at least one Collider
    Vector2 FindNearestPlatform()
    {
        Vector2 newRight = transform.right;
        newRight.y = 1;
        int length = Physics2D.OverlapBox(myRb2D.position + (platformDetectOffset * newRight), platformDetectSize, 0.0f, jumpingFilter, listOfPlatforms);
        Debug.Log("Found: " + length);
        if (length == 0)
            return Vector2.zero;

        // Return the closest after filtering
        return FilterPlatform();
    }
    // Fills the list with platforms that are within my Collider
    // Returns Vector2.zero if no Colliders Found
    // Returns the Position if found at least one Collider
    Vector2 FindFurtherestPlatform()
    {
        Vector2 newRight = transform.right;
        newRight.y = 1;
        int length = Physics2D.OverlapBox(myRb2D.position + (platformDetectOffset * newRight), platformDetectSize, 0.0f, jumpingFilter, listOfPlatforms);
        Debug.Log("Found: " + length);
        if (length == 0)
            return Vector2.zero;

        // Return the closest after filtering
        return FilterPlatform(false);
    }
    // Removes all the platfrom from the list
    // Except for the furtherest of Closest depending on what you pass in
    Vector2 FilterPlatform(bool closet = true)
    {
        int closetIndex = 0;
        float dist = ((Vector2)listOfPlatforms[closetIndex].gameObject.transform.position - myRb2D.position).sqrMagnitude;
        float testingDist = 0;
        for (int i = 1; i < listOfPlatforms.Count; ++i)
        {
            // get closest platform
            testingDist = ((Vector2)listOfPlatforms[i].gameObject.transform.position - myRb2D.position).sqrMagnitude;
            // Closest or Furtherest
            if(closet)
            {
                if (testingDist < dist)
                {
                    closetIndex = i;
                    dist = testingDist;
                }
            }
            else
            {
                if (testingDist > dist)
                {
                    closetIndex = i;
                    dist = testingDist;
                }
            }
            
        }

        return listOfPlatforms[closetIndex].gameObject.transform.position;
    }
    // Checks the height difference between the egg and this enemy
    // Returns true if a change in state has been made
    // Returns false if no change has been made
    bool CheckHeightDifference()
    {
        float diff = myRb2D.position.y - Egg.Instance.GetPosition().y;
        if (diff <= 2.0f)
        {
            if(currentState != STATES.S_EGG_SIMILARHEIGHT)
            {
                currentState = STATES.S_EGG_SIMILARHEIGHT;
                return true;
            }
        }
        else
        {
            if(currentState != STATES.S_EGG_DIFFERENTHEIGHT)
            {
                currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                return true;
            }
        }

        return false;
    }


    // Custom Move Function as we need to return a value
    // Returns false when can no longer move
    // Returns true if can still move
    private bool MoveWolf(bool checkBelow = true)
    {
        // set the new direction
        moveDirection = moveTargetPos - myRb2D.position;
        moveDirection.y = 0.0f;
        moveDirection.Normalize();

        // Do we need to check if we can drop down
        if(checkBelow)
        {
            // Check if we can even move
            if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength)))
            {
                // Move
                myRb2D.MovePosition(myRb2D.position + (moveDirection * moveSpeed * Time.deltaTime));
                return true;
            }
            return false;
        }

        // Move
        myRb2D.MovePosition(myRb2D.position + (moveDirection * moveSpeed * Time.deltaTime));
        return true;
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
        moveDirection = -moveDirection;
        moveTargetPos.Set(myRb2D.position.x + (moveDirection.x * 100.0f), moveTargetPos.y);
    }
    // Shooting Logic
    private void Shoot()
    {
        GameObject newProj = ObjectPooler.Instance.FetchGO_Pos(projectilePrefab.name, shootingPos.position);

        Vector2 launchVelocity = Vector2.zero;
        launchVelocity.x = (targetObject.transform.position.x - shootingPos.position.x) * timeToHitTarget;    // Initial velocity in X axis
        launchVelocity.y = -(-(targetObject.transform.position.y - shootingPos.position.y) + 0.5f * Physics2D.gravity.y * timeToHitTarget * timeToHitTarget) * timeToHitTarget;

        // Add the velocity to the object
        newProj.GetComponent<Rigidbody2D>().velocity = launchVelocity;
    }
    // Jumping Logic
    private void JumpWolf(Vector2 newTarget)
    {
        Vector2 launchVelocity = Vector2.zero;
        launchVelocity.x = (newTarget.x - myRb2D.position.x) * timeToHitTarget;    // Initial velocity in X axis
        launchVelocity.y = -(-(newTarget.y - myRb2D.position.y) + 0.5f * Physics2D.gravity.y * timeToHitTarget * timeToHitTarget) * timeToHitTarget;

        // Add the velocity to enemy
        myRb2D.velocity = Vector2.zero; 
        myRb2D.velocity = launchVelocity;

        groundCheckScript.startCheck = true;
        isGrounded = false;
    }
    // Grounded Logic
    public void SetGrounded()
    {
        isGrounded = true;
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

        Gizmos.color = Color.blue;
        Vector3 tempo = transform.right;
        tempo.y = 1;
        Gizmos.DrawWireCube((Vector2)transform.position + (platformDetectOffset * tempo), platformDetectSize);
    }
}
