using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    // States
    public enum STATES
    {
        S_EGG_SIMILARHEIGHT,
        S_EGG_DIFFERENTHEIGHT,
        S_EGG_ONTOP,

        S_FOUNDPLAYER,
        S_WALKBACK,
        S_RUNAWAY,

        S_SHOOT_EGG,

        S_SHOOT_PLAYER,
        S_WALK_PLAYER,
    }
    public STATES currentState;
    [System.Serializable]
    public class DetectBox
    {
        public Vector2 detectOffset;
        public Vector2 detectSize;
    }
    #region Data
    [Header("SquirrelWolf Class")]
    [SerializeField]
    // How far to detect for player
    float playerDetectionRange = 0.0f;
    Collider2D result = null;
    [Header("Jumping")]
    [SerializeField]
    DetectBox sideTopDetect = new DetectBox();   // How far to detect for jumping platform to our side and top
    [SerializeField]
    DetectBox bottomDetect = new DetectBox();   // How far to detect for jumping platform for bottom
    List<Collider2D> listOfPlatforms = new List<Collider2D>();    // Used to store the platforms that we can jump to
    ContactFilter2D jumpingFilter = new ContactFilter2D();      // To prevent me calling new everytime
    bool isGrounded = false;     // Used to check if we have reached the ground
    [SerializeField]
    groundCheck groundCheckScript = null;   // Script used to check if have reached the ground when jumping
    [Header("GroundCasting")]
    [SerializeField]
    // Ground Cast
    Transform groundCast = null;
    [SerializeField]
    float groundCastLength = 0.09f;
    RaycastHit2D rayhit2D = new RaycastHit2D();

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
    [Header("RunAwayTimer")]
    [SerializeField]
    float fleeTime = 5.0f;
    float fleeTimer = 0.0f;
    // Unity Stuff
    // Get referrence to the target Object
    GameObject targetObject = null;
    #endregion


    private void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = STATES.S_EGG_DIFFERENTHEIGHT;

        // Convert int to floats for easier calculation
        timeToHitTarget = 1 / timeToHitTarget;
        // Set the Flee Timer
        fleeTimer = fleeTime;

        // Set the jumping Filters
        jumpingFilter.SetLayerMask(LayerMask.GetMask("JumpPointLayer"));
        jumpingFilter.ClearDepth();
        jumpingFilter.useTriggers = true;

        // Multiply ranges with scale
        // Platform detecting
        sideTopDetect.detectOffset *= transform.localScale;
        sideTopDetect.detectSize *= transform.localScale;
        // Player detect
        playerDetectionRange *= transform.localScale.x;
        // Shooting
        maxShootingRange *= transform.localScale.x;
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
                    // If not yet grounded, return;
                    if (!isGrounded)
                        return;

                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if(targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_FOUNDPLAYER;
                        return;
                    }


                    // set the position of egg and move there
                    SetNewObjectTarget(Egg.Instance.gameObject);
                    // check if we can shoot projectile at egg
                    if ((moveTargetPos - myRb2D.position).sqrMagnitude <= (maxShootingRange * maxShootingRange))
                    {
                        currentState = STATES.S_SHOOT_EGG;
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
                            // Set new target and jump there
                            moveTargetPos = platformEdgePos;
                            JumpWolf(moveTargetPos);
                        }

                        return;
                    }

                    // Check if we can attack using melee
                    //if ((moveTargetPos - myRb2D.position).sqrMagnitude < meleeAttackDistance)
                    //    currentState = STATES.S_MELEE;
                }
                break;
            case STATES.S_EGG_DIFFERENTHEIGHT:
                {
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

                    // set the target as egg
                    SetNewObjectTarget(Egg.Instance.gameObject);
                    // Move enemy
                    if(!MoveWolf())
                    {
                        Vector2 platformEdgePos = FindPlatformBelow();
                        if (platformEdgePos != Vector2.zero)
                        {
                            // Set new target and jump there
                            SetNewPosTarget(platformEdgePos);
                            JumpWolf(moveTargetPos);
                            return;
                        }
                        else
                            MoveWolf(false);
                    }

                    // Once close enough Y pos, 
                    float posDiff = myRb2D.position.y - targetObject.transform.position.y;
                    if (posDiff < 2.0f)
                    {
                        currentState = STATES.S_EGG_SIMILARHEIGHT;
                        return;
                    }
                    // if we are stuck, then we should go to one of the points
                    posDiff = targetObject.transform.position.x - myRb2D.position.x;
                    if (posDiff < 0.5f && posDiff > -0.5f)
                    {
                        currentState = STATES.S_EGG_ONTOP;
                        // Go to the closest position from the egg WITH AN OFFSET
                        moveTargetPos = groundCheckScript.platformStandingOn.GetClosestPosition(moveTargetPos);
                        moveDirection = moveTargetPos - myRb2D.position;
                    }
                    
                }
                break;
            case STATES.S_EGG_ONTOP:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if (targetObject != null)    // Found Player
                    {
                        currentState = STATES.S_FOUNDPLAYER;
                        return;
                    }

                    // Keep moving until we drop
                    MoveWolf(false);
                    if (!isGrounded)
                        currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                }
                break;

            case STATES.S_FOUNDPLAYER:  // Assume that targetObject here will always be player
                {
                    // check if we can ATTACK player or need to walk there
                    int chance = Random.Range(1, 1);

                    // Flee from player
                    if(chance == 0)
                    {
                        currentState = STATES.S_RUNAWAY;    // get the furtherest Point from the player on the platform we are standing on
                        SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(targetObject.transform.position));
                    }
                    else
                    {
                        // Attack player
                        currentState = STATES.S_WALK_PLAYER;
                        SetNewObjectTarget(targetObject);
                    }
                    
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
                    if (!isGrounded)
                        return;

                    // Check if we can jump up more
                    Vector2 platformEdgePos = FindPlatformAbove();
                    if(platformEdgePos != Vector2.zero)
                    {
                        // Set new target and jump there
                        SetNewPosTarget(platformEdgePos);
                        JumpWolf(moveTargetPos);
                        return;
                    }


                    // Move enemy Horizontal
                    if (!MoveWolf())
                    {
                        // If we don't find any platforms to jump to, then we turn back
                        platformEdgePos = FindFurtherestPlatform();
                        if (platformEdgePos == Vector2.zero)
                        {
                            // Go back to finding statew
                            //currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                            myRb2D.velocity = Vector2.zero;
                        }
                        else
                        {
                            // Set new target and jump there
                            SetNewPosTarget(platformEdgePos);
                            JumpWolf(moveTargetPos);
                            return;
                        }
                    }

                    fleeTimer -= Time.deltaTime;
                    if (fleeTimer < 0.0f)
                    {
                        currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        fleeTimer = fleeTime;
                    }
                }
                break;


            case STATES.S_SHOOT_EGG:
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


            case STATES.S_WALK_PLAYER:
                {
                    // check if target object is out of range
                    if(((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude > (playerDetectionRange * playerDetectionRange))
                    {
                        currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        return;
                    }


                    // check if we can melee the player


                    // check if we can shoot projectile at player
                    if ((moveTargetPos - myRb2D.position).sqrMagnitude <= (maxShootingRange * maxShootingRange))
                    {
                        currentState = STATES.S_SHOOT_PLAYER;
                        return;
                    }

                    // Walk towards player
                    SetNewObjectTarget(targetObject);
                    MoveWolf();
                }
                break;
            case STATES.S_SHOOT_PLAYER:
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer < 0.0f)
                    {
                        // Shoot
                        Shoot();
                        // Reset Timer
                        attackTimer = attackTime;

                        // Is a player in range?
                        if (!IsPlayerStillInRange())
                            currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                    }
                }
                break;
        }
    }

    // Returns the player Object if he is in Range
    // Player must be in Player Layer and Tag
    // Does not count EGG as player
    GameObject IsPlayerInRange()
    {
        // Get if we have hit anything, player or egg
        result = Physics2D.OverlapCircle(myRb2D.position, playerDetectionRange, LayerMask.GetMask("Player"));
        if (result == null)
            return null;
        if (result.gameObject.tag != "Player")
            return null;

        return result.gameObject;
    }
    // Returns if player object is still in range
    bool IsPlayerStillInRange()
    {
        // check if player is still in range
        if (((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude > playerDetectionRange)
            return false;
        return true;
    }

    // Fills the list with platforms that are within the sideTopDetect
    // Returns Vector2.zero if no Colliders Found
    // Returns the Position if found at least one Collider
    int SearchPlatforms()
    {
        Vector2 newRight = transform.right;
        newRight.y = 1;
        int length = Physics2D.OverlapBox(myRb2D.position + (sideTopDetect.detectOffset * newRight), sideTopDetect.detectSize, 0.0f, jumpingFilter, listOfPlatforms);
        Debug.Log("Found: " + length);
        return length;
    }
    // Fills the list with platforms that are within the bottomDetect
    // Returns Vector2.zero if no Colliders Found
    // Returns the Position if found at least one Collider
    int SearchPlatforms_Below()
    {
        Vector2 newRight = transform.right;
        newRight.y = 1;
        int length = Physics2D.OverlapBox(myRb2D.position + (bottomDetect.detectOffset * newRight), bottomDetect.detectSize, 0.0f, jumpingFilter, listOfPlatforms);
        Debug.Log("BottomFound: " + length);
        return length;
    }
    // Finds platforms and returns Closest platform
    Vector2 FindNearestPlatform()
    {
        // Find the platforms
        if (SearchPlatforms() == 0)
            return Vector2.zero;
        // Return the closest after filtering
        return FilterPlatformDistance();
    }
    // Finds platforms and returns Furtherest platform
    Vector2 FindFurtherestPlatform()
    {
        // Find the platforms
        if (SearchPlatforms() == 0)
            return Vector2.zero;
        // Return the closest after filtering
        return FilterPlatformDistance(false);
    }
    // Finds platforms and returns Highest platform
    Vector2 FindPlatformAbove()
    {
        // Find the platforms
        if (SearchPlatforms() == 0)
            return Vector2.zero;

        // get the highest platform
        int selectedIndex = -1;
        float yPos = 0;
        Vector2 testDirection = Vector2.zero;
        Vector3 platformPos;
        for (int i = 0; i < listOfPlatforms.Count; ++i)
        {
            // Set the pos
            platformPos = listOfPlatforms[i].gameObject.transform.position;
            // Can I even jump there? or is it blocked by the platform itself
            testDirection = (platformPos - shootingPos.position);
            rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.y, LayerMask.GetMask("Ground"));
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.yellow);
            if (rayhit2D.collider != null)   // We hit smth
                continue;
            if (platformPos.y - myRb2D.position.y < 0.5f) // If we are on the same y
                continue;

            if(platformPos.y > yPos)
            {
                selectedIndex = i;
                yPos = platformPos.y;
            }
        }

        if (selectedIndex == -1)
            return Vector2.zero;
        return listOfPlatforms[selectedIndex].gameObject.transform.position;
    }
    // Finds platforms and returns Lowest platform
    Vector2 FindPlatformBelow()
    {
        // Find the platforms
        if (SearchPlatforms_Below() == 0)
            return Vector2.zero;

        // get the highest platform
        int selectedIndex = -1;
        float yPos = Mathf.Infinity;
        Vector3 platformPos;
        for (int i = 0; i < listOfPlatforms.Count; ++i)
        {
            // Set the pos
            platformPos = listOfPlatforms[i].gameObject.transform.position;
            // Can I even jump there? or is it blocked by the platform itself
            //testDirection = (platformPos - shootingPos.position);
            //rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.y, LayerMask.GetMask("Ground"));
            //Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.yellow);
            //if (rayhit2D.collider != null)   // We hit smth
            //    continue;
            if (myRb2D.position.y - platformPos.y < 0.5f) // If we are on the same y
                continue;

            if (platformPos.y < yPos)
            {
                selectedIndex = i;
                yPos = platformPos.y;
            }
        }

        if (selectedIndex == -1)
            return Vector2.zero;
        return listOfPlatforms[selectedIndex].gameObject.transform.position;
    }
    // Removes all the platfrom from the list
    // Except for the furtherest of Closest depending on what you pass in
    Vector2 FilterPlatformDistance(bool closet = true)
    {
        int selectedIndex = -1;
        float dist;
        if (closet) // set differently compare values
            dist = Mathf.Infinity;
        else
            dist = 0.0f;
        float testingDist = 0;
        Vector2 testDirection = Vector2.zero;


        for (int i = 0; i < listOfPlatforms.Count; ++i)
        {
            // Can I even jump there? or is it blocked by the platform itself
            testDirection = (listOfPlatforms[i].gameObject.transform.position - shootingPos.position);
            rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.x, LayerMask.GetMask("Ground"));
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.x, Color.yellow);
            if (rayhit2D.collider != null)   // We hit smth
                continue;


            // Check if distance is more than currently selected platform
            testingDist = testDirection.sqrMagnitude;
            // Closest or Furtherest
            if(closet)
            {
                if (testingDist < dist)
                {
                    selectedIndex = i;
                    dist = testingDist;
                }
            }
            else
            {
                if (testingDist > dist)
                {
                    selectedIndex = i;
                    dist = testingDist;
                }
            }
            
        }

        // If we found a platform we can jump to
        if(selectedIndex != -1)
            return listOfPlatforms[selectedIndex].gameObject.transform.position;
        return Vector2.zero;
    }



    // Checks if you are have reached your target
    bool ReachedTarget()
    {
        if ((myRb2D.position - moveTargetPos).sqrMagnitude < 1.0f)
            return true;

        return false;
    }
    // Set a new target to move towards to
    // Also recalculates the direction for you
    void SetNewObjectTarget(GameObject newtarget)
    {
        // set the data
        targetObject = newtarget;
        moveTargetPos = newtarget.transform.position;
        // set the direction
        moveDirection = moveTargetPos - myRb2D.position;
    }
    void SetNewPosTarget(Vector2 newtarget)
    {
        // set the data
        moveTargetPos = newtarget;
        // set the direction
        moveDirection = moveTargetPos - myRb2D.position;
    }
    // Custom Move Function as we need to return a value
    // Returns false when can no longer move
    // Returns true if can still move
    private bool MoveWolf(bool checkBelow = true)
    {
        // set the new direction
        //moveDirection = moveTargetPos - myRb2D.position;
        if (!isGrounded)
            moveDirection.y = Physics2D.gravity.y;
        else
            moveDirection.y = 0.0f;
        moveDirection.Normalize();
        // TESTING FLIP
        FlipEnemy();

        // Do we need to check if we can drop down
        if(checkBelow)
        {
            // Cast below us
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
    public void LeftGrounded()
    {
        isGrounded = false;
    }
    // What to do when grounded
    public void HitGround()
    {
        switch (currentState)
        {
            case STATES.S_RUNAWAY:
                {
                    // Walk to end of platform
                    if(Random.Range(1,1) == 0) // random chance to turn around
                        SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(myRb2D.position));
                    else    // Turn around
                    {
                        TurnWolfAround();
                        Vector2 platformPos = FindPlatformAbove();
                        if(platformPos == Vector2.zero)
                        {
                            TurnWolfAround();
                            SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(myRb2D.position));
                        }else
                        {
                            SetNewPosTarget(platformPos);
                            JumpWolf(platformPos);
                        }
                    }

                }
                break;
        }

    }


    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        spawningZone = newSpawnZone;
        // Reset Position and Velocity
        transform.position = newPos;
        myRb2D.position = newPos;
        myRb2D.velocity = Vector2.zero;
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
        Gizmos.DrawWireCube((Vector2)transform.position + (sideTopDetect.detectOffset * tempo), sideTopDetect.detectSize);
        Gizmos.DrawWireCube((Vector2)transform.position + (bottomDetect.detectOffset * tempo), bottomDetect.detectSize);
    }
}
