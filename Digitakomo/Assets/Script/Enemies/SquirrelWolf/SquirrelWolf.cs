using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    #region Classes and Enums
    // States for StateMachine
    public enum STATES
    {
        S_EGG_SIMILARHEIGHT,
        S_EGG_DIFFERENTHEIGHT,
        S_EGG_ONTOP,
        S_WALKBACK,
        S_RUNAWAY,

        S_SHOOT_EGG,
        S_MELEE_EGG,

        S_FOUNDPLAYER,
        S_SHOOT_PLAYER,
        S_MELEE_PLAYER,
        S_WALK_PLAYER,
    }
    public STATES currentState;
    // Which way of attacking are we using
    public enum ATTACK
    {
        A_MELEE,
        A_SHOOT
    }
    public ATTACK attackMethod = ATTACK.A_SHOOT;
    [System.Serializable]   // Used for platform detection
    public class DetectBox
    {
        public Vector2 detectOffset;
        public Vector2 detectSize;
    }
    #endregion

    [Header("SquirrelWolf Class")]
    [SerializeField]
    // How far to detect for player
    float playerDetectionRange = 0.0f;
    List<Collider2D> listOfPlayers = new List<Collider2D>();
    ContactFilter2D playerFilter = new ContactFilter2D();      // To prevent me calling new everytime
    #region Jumping
    [Header("Jumping")]
    [SerializeField]
    DetectBox sideTopDetect = new DetectBox();   // How far to detect for jumping platform to our side and top
    [SerializeField]
    DetectBox bottomDetect = new DetectBox();   // How far to detect for jumping platform for bottom
    List<Collider2D> listOfPlatforms = new List<Collider2D>();    // Used to store the platforms that we can jump to
    ContactFilter2D jumpingFilter = new ContactFilter2D();      // To prevent me calling new everytime
    bool isGrounded = false;     // Used to check if we have reached the ground
    bool isJumped = false;       // Used to check if we are jumping
    static float YPosDifference = 2.0f; // The difference to check before we change state
    [SerializeField]
    groundCheck groundCheckScript = null;   // Script used to check if have reached the ground when jumping
    [Header("GroundCasting")]
    [SerializeField]
    // Ground Cast
    Transform groundCast = null;
    [SerializeField]
    float groundCastLength = 0.09f;
    RaycastHit2D rayhit2D = new RaycastHit2D();
    #endregion

    #region Melee
    [Header("Melee")]
    [SerializeField]    // The minimum range before start to melee
    float meleeDistance = 2.0f;
    bool meleeDoneAnimation = false;    // Used to check if we have finished the Attack Animations
    #endregion
    #region Shooting
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
    bool shootingDoneAnimation = false;    // Used to check if we have finished the Attack Animations
    #endregion
    #region RunAway
    [Header("RunAwayTimer")]
    [SerializeField]
    float fleeTime = 5.0f;
    float fleeTimer = 0.0f;
    // have we been attacked?
    bool playerAttackedWolf = false;
    #endregion
    #region Frozen
    bool isFrozen = false;
    #endregion
    // Unity Stuff
    // Get referrence to the target Object
    GameObject targetObject = null;


    private void Awake()
    {
        // Call baseClass's Init
        Init();
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
        // Set the player Filters
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
        playerFilter.ClearDepth();

        // Calculate the percentage difference to scale the colliders
        Vector2 percentageDifference = Vector2.zero;
        percentageDifference.x = transform.localScale.x / 3.25f;
        percentageDifference.y = transform.localScale.y / 7.0975f;
        // Multiply ranges with scale
        // Platform detecting
        sideTopDetect.detectOffset *= percentageDifference;
        sideTopDetect.detectSize *= percentageDifference;
        bottomDetect.detectOffset *= percentageDifference;
        bottomDetect.detectSize *= percentageDifference;
        // Player detect
        playerDetectionRange *= percentageDifference.x;
        // Shooting
        maxShootingRange *= percentageDifference.x;
        // Melee
        meleeDistance *= percentageDifference.x;

    }

    // Update is called once per frame
    void Update()
    {
        // Status Effects
        seManager.Update();
        return;
        // if I am frozen, return
        if (isFrozen)
            return;
        // Update State Machine
        UpdateStates();
    }


    // Used to define this AI's logic
    void UpdateStates()
    {
        // if we are jumping, then don't do anything
        if (!isGrounded || isJumped)
        {
            // Are we falling?
            if (myRb2D.velocity.y < 0.0f)
            {
                isJumped = false;
                myAnimator.SetBool("mb_Fall", true);
            }
                
            return;
        }
            


        switch (currentState)
        {
            case STATES.S_EGG_SIMILARHEIGHT:
                {
                    // If not yet grounded, return;
                    if (!isGrounded)
                        return;

                    // Is a player in range?
                    if (IsPlayerInRange())
                        return;


                    // set the position of egg and move there
                    SetNewObjectTarget(Egg.Instance.gameObject);
                    // Check if we can attack depending on what attacking method
                    float distance = (moveTargetPos - myRb2D.position).sqrMagnitude;
                    switch (attackMethod)
                    {
                        case ATTACK.A_MELEE:
                            {
                                if (distance <= (meleeDistance * meleeDistance))
                                {
                                    currentState = STATES.S_MELEE_EGG;
                                    StopVel();
                                    myAnimator.SetBool("mb_Melee", true);
                                    return;
                                }
                            }
                            break;
                        case ATTACK.A_SHOOT:
                            {
                                // check if we can shoot projectile at egg
                                if (distance <= (maxShootingRange * maxShootingRange))
                                {
                                    currentState = STATES.S_SHOOT_EGG;
                                    StopVel();
                                    myAnimator.SetBool("mb_Shoot", true);
                                    return;
                                }
                            }
                            break;
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

                            // Check if we can attack using range
                        }
                        else
                        {
                            // Set new target and jump there
                            moveTargetPos = platformEdgePos;
                            JumpWolf(moveTargetPos);
                        }

                        return;
                    }
                }
                break;
            case STATES.S_EGG_DIFFERENTHEIGHT:
                {
                    // Is a player in range?
                    if (IsPlayerInRange())
                        return;

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
                    if (posDiff < YPosDifference)
                    {
                        currentState = STATES.S_EGG_SIMILARHEIGHT;
                        // Randomise the attacking method
                        //attackMethod = (ATTACK)Random.Range((int)ATTACK.A_MELEE, (int)ATTACK.A_SHOOT + 1);
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
            case STATES.S_EGG_ONTOP:    // When we enter this state, we should already have a target to move to
                {
                    // Is a player in range?
                    if (IsPlayerInRange())
                        return;

                    // Keep moving until we can't move anymore,
                    // then jump
                    if (!MoveWolf())
                    {
                        Vector2 platformEdgePos = FindPlatformBelow();
                        if (platformEdgePos != Vector2.zero)
                        {
                            // Set new target and jump there
                            SetNewPosTarget(platformEdgePos);
                            JumpWolf(moveTargetPos);
                            currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                            return;
                        }
                    }
                }
                break;
            case STATES.S_WALKBACK:
                {
                    // Is a player in range?
                    if (IsPlayerInRange())
                        return;

                    // Move enemy
                    if (!MoveWolf())
                    {
                        TurnWolfAround();
                        return;
                    }
                }
                break;
            


            case STATES.S_MELEE_EGG:
                {
                    // Check if done animation
                    if (meleeDoneAnimation)
                    {
                        // Reset bool
                        meleeDoneAnimation = false;

                        // if we were hit by player
                        if (playerAttackedWolf)
                        {
                            // Is a player in range?
                            if (IsPlayerInRange())
                            { 
                                SetNewObjectTarget(targetObject);
                            }
                            else
                            {
                                // Run to nearest platform edge, since no player
                                currentState = STATES.S_RUNAWAY;
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(myRb2D.position));
                            }
                            myAnimator.SetBool("mb_Melee", false);
                            playerAttackedWolf = false; // Reset attecked bool
                        }
                        //else
                        //{
                        //    // Found Player
                        //    if (targetObject != null)
                        //    {
                        //        currentState = STATES.S_FOUNDPLAYER;
                        //        myAnimator.SetBool("mb_Melee", false);
                        //    }
                        //}
                        
                    }
                }
                break;
            case STATES.S_SHOOT_EGG:
                {
                    if(shootingDoneAnimation)
                    {
                        // Reset Bool
                        shootingDoneAnimation = false;

                        // if we were hit by player
                        if (playerAttackedWolf)
                        {
                            // Is a player in range?
                            if (IsPlayerInRange())  // We found a player in range, Might not be the one who fired the shot
                            {
                                SetNewObjectTarget(targetObject);
                            }
                            else
                            {
                                // Run to nearest platform edge, since no player
                                currentState = STATES.S_RUNAWAY;
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetClosestPosition(myRb2D.position));
                            }
                            myAnimator.SetBool("mb_Shoot", false);
                            playerAttackedWolf = false; // Reset attecked bool
                        }
                        //else
                        //{
                        //    // Found Player
                        //    if (targetObject != null)   
                        //    {
                        //        currentState = STATES.S_FOUNDPLAYER;
                        //        myAnimator.SetBool("mb_Shoot", false);
                        //    }
                        //}
                    }
                }
                break;


            case STATES.S_FOUNDPLAYER:  // Assume that targetObject here will always be player
                {
                    // check if we can ATTACK player or need to walk there
                    int chance = Random.Range(1, 1);

                    // Flee from player
                    if (chance == 0)
                    {
                        currentState = STATES.S_RUNAWAY;    // get the furtherest Point from the player on the platform we are standing on
                        // SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(targetObject.transform.position));

                        TurnWolfAround();
                        switch (facingDirection)
                        {
                            case DIRECTION.D_LEFT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetLeftPoint());
                                break;
                            case DIRECTION.D_RIGHT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetRightsPoint());
                                break;
                        }
                    }
                    else
                    {
                        // Attack player
                        currentState = STATES.S_WALK_PLAYER;
                        SetNewObjectTarget(targetObject);
                        // Randomise way of attacking
                        //attackMethod = (ATTACK)Random.Range((int)ATTACK.A_MELEE, (int)ATTACK.A_SHOOT+1);
                    }


                }
                break;
            case STATES.S_RUNAWAY:
                {
                    // Check if we can jump up more
                    Vector2 platformEdgePos = FindPlatformAbove();
                    if (platformEdgePos != Vector2.zero)
                    {
                        // Set new target and jump there
                        SetNewPosTarget(platformEdgePos);
                        JumpWolf(moveTargetPos);
                        return;
                    }

                    // Move enemy Horizontal
                    if (!MoveWolf())
                    {
                        // If we don't find any platforms to jump to, then we just stay put
                        platformEdgePos = FindFurtherestPlatform();
                        if (platformEdgePos == Vector2.zero)
                        {
                            // Go back to finding statew
                            //currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                            StopVel();
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
            case STATES.S_WALK_PLAYER:
                {
                    // check if player object is out of range
                    float playerDist = 0.0f;
                    if(!IsTargetObjStillInRange(ref playerDist))
                    {
                        float posDiff = myRb2D.position.y - Egg.Instance.transform.position.y;
                        if (posDiff < YPosDifference)
                            currentState = STATES.S_EGG_SIMILARHEIGHT;
                        else
                            currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        return;
                    }


                    // Walk towards player
                    SetNewObjectTarget(targetObject);
                    // What attack type are we using
                    switch (attackMethod)
                    {
                        case ATTACK.A_MELEE:
                            {
                                // Move until we can't   
                                if (!MoveWolf())
                                {
                                    // Do we need to jump pass a platform?
                                    Vector2 nextPos = FindNearestPlatform();
                                    if (nextPos != Vector2.zero)
                                    {
                                        // Is the next platform very close, then just jump there
                                        //if ((myRb2D.position - nextPos).sqrMagnitude < 5.0f)
                                        //{
                                            SetNewPosTarget(nextPos);
                                            JumpWolf(nextPos);
                                            return;
                                        //}
                                    }


                                    // maybe can check the distance if very small then can follow
                                    currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                                }

                                // check if we can melee the player
                                if (playerDist < (meleeDistance * meleeDistance))
                                {
                                    currentState = STATES.S_MELEE_PLAYER;
                                    StopVel();
                                    myAnimator.SetBool("mb_Melee", true);
                                    return;
                                }
                            }
                            break;
                        case ATTACK.A_SHOOT:
                            {
                                // check if we can shoot projectile at player
                                if (playerDist <= (maxShootingRange * maxShootingRange))
                                {
                                    currentState = STATES.S_SHOOT_PLAYER;
                                    StopVel();
                                    myAnimator.SetBool("mb_Shoot", true);
                                    return;
                                }

                                // Move until we can't   
                                if (!MoveWolf())
                                {
                                    // change attack method to melee since we can't shoot the player
                                    attackMethod = ATTACK.A_MELEE;
                                }
                            }
                            break;
                    }  
                }
                break;
            case STATES.S_SHOOT_PLAYER:
                {
                    if (shootingDoneAnimation)
                    {
                        // Reset Bool
                        shootingDoneAnimation = false;


                        // Is a player in range?
                        //float temp = 0.0f;
                        //if (!IsPlayerStillInRange(ref temp))
                        //{
                        //    currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        //    myAnimator.SetBool("mb_Shoot", false);
                        //}

                        currentState = STATES.S_RUNAWAY;
                        myAnimator.SetBool("mb_Shoot", false);
                        TurnWolfAround();
                        switch (facingDirection)
                        {
                            case DIRECTION.D_LEFT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetLeftPoint());
                                break;
                            case DIRECTION.D_RIGHT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetRightsPoint());
                                break;
                        }
                    }
                }
                break;
            case STATES.S_MELEE_PLAYER:
                {
                    // Check done animation
                    if (meleeDoneAnimation)
                    {
                        // Reset bool
                        meleeDoneAnimation = false;

                        //// Is a player in range?
                        //float temp = 0.0f;
                        //if (!IsPlayerStillInRange(ref temp))
                        //{
                        //    currentState = STATES.S_EGG_DIFFERENTHEIGHT;
                        //    myAnimator.SetBool("mb_Melee", false);
                        //}

                        currentState = STATES.S_RUNAWAY;
                        myAnimator.SetBool("mb_Melee", false);
                        TurnWolfAround();
                        switch (facingDirection)
                        {
                            case DIRECTION.D_LEFT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetLeftPoint());
                                break;
                            case DIRECTION.D_RIGHT:
                                SetNewPosTarget(groundCheckScript.platformStandingOn.GetRightsPoint());
                                break;
                        }
                        
                    }
                }
                break;
        }
    }

    // Returns bool if player is in Range
    // Player must be in Player Layer and Tag
    // Does not count EGG as player
    // Also changes state for you
    bool IsPlayerInRange()
    {
        // Get if we have hit anything, player or egg
        Physics2D.OverlapCircle(myRb2D.position, playerDetectionRange, playerFilter, listOfPlayers);

        foreach (Collider2D result in listOfPlayers)
        {
            if (result == null)
                continue;
            if (result.gameObject.tag != "Player")
                continue;
            // check are we actually in range or we just hit the collider
            if (((Vector2)result.gameObject.transform.position - myRb2D.position).sqrMagnitude > (playerDetectionRange * playerDetectionRange))
                continue;
            // raycast to check if we can actually go to player
            Vector2 testDirection = (Vector2)result.gameObject.transform.position - myRb2D.position;
            rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.x, LayerMask.GetMask("Ground"));

            if (rayhit2D.collider != null)  // If we hit smth, return null
            {
                Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.yellow);
                continue;
            }
            else
                Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.red);


            targetObject = result.gameObject;
            currentState = STATES.S_FOUNDPLAYER;
            StopVel();
            return true;
        }

        return false;
    }
    // Returns if target object is still in range
    bool IsTargetObjStillInRange(ref float distanceReturned)
    {
        // check if player is still in range
        distanceReturned = ((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude;
        if (distanceReturned > (playerDetectionRange * playerDetectionRange))
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
           
            // Check dot product
            Vector2 horizontal = testDirection.normalized;
            horizontal.y = 0.0f;
            float top = Vector2.Dot(testDirection.normalized, horizontal);
            float angle = Mathf.Acos(top / 1);
            //Debug.LogWarning("Angle: " + (90 - (Mathf.Rad2Deg * angle)));
            if (1.5708f - angle <= 0.139626f)   // 1.5708 = 90.0Deg, 0.139626 = 8.0Deg
                continue;
            // Check Ray cast
            rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.y, LayerMask.GetMask("Ground"));
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.yellow);
            if (rayhit2D.collider != null)   // We hit smth
                continue;

            if (platformPos.y - myRb2D.position.y < YPosDifference) // If we are on the same y
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
        Vector2 testDirection;

        for (int i = 0; i < listOfPlatforms.Count; ++i)
        {
            // Set the pos
            platformPos = listOfPlatforms[i].gameObject.transform.position;
            // Can I even jump there? or is it blocked by the platform itself
            testDirection = (platformPos - shootingPos.position);
            rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, sideTopDetect.detectSize.y, LayerMask.GetMask("Ground"));
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * sideTopDetect.detectSize.y, Color.yellow);
            //if (rayhit2D.collider != null)   // We hit smth
            //    continue;
            if (myRb2D.position.y - platformPos.y < YPosDifference) // If we are on the same y
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
    // Moves in the direction set
    // Returns false when can no longer move
    // Returns true if can still move
    private bool MoveWolf(bool checkBelow = true)
    {
        // set the new direction
        if (!isGrounded)
        {
            moveDirection.Normalize();
            moveDirection.y = Physics2D.gravity.y;
        }
        else
        {
            moveDirection.y = 0.0f;
            moveDirection.Normalize();
        }
            
        // TESTING FLIP
        FlipEnemy();

        // Do we need to check if we can drop down
        if(checkBelow)
        {
            // Cast below us
            // Check if we can even move
            if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength), LayerMask.GetMask("Ground")))
            {
                // Move
                myRb2D.MovePosition(myRb2D.position + (moveDirection * moveSpeed * Time.deltaTime));
                myAnimator.SetBool("mb_Move", true);
                return true;
            }
            myAnimator.SetBool("mb_Move", false);
            return false;
        }

        // Move
        myRb2D.MovePosition(myRb2D.position + (moveDirection * moveSpeed * Time.deltaTime));
        myAnimator.SetBool("mb_Move", true);
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
    public void Shoot()
    {
        GameObject newProj = ObjectPooler.Instance.FetchGO_Pos(projectilePrefab.name, shootingPos.position);

        Vector2 launchVelocity = Vector2.zero;
        launchVelocity.x = (targetObject.transform.position.x - shootingPos.position.x) * timeToHitTarget;    // Initial velocity in X axis
        launchVelocity.y = -(-(targetObject.transform.position.y - shootingPos.position.y) + 0.5f * Physics2D.gravity.y * timeToHitTarget * timeToHitTarget) * timeToHitTarget;

        // Add the velocity to the object
        newProj.GetComponent<Rigidbody2D>().velocity = launchVelocity;
    }
    public void DoneShoot()
    {
        shootingDoneAnimation = true;
    }
    // Melee Logic
    public void Melee()
    {
        // Check distance here again, then if player too far, then don't do damge..
        if (((Vector2)targetObject.gameObject.transform.position - myRb2D.position).sqrMagnitude > (meleeDistance * meleeDistance))
            return;

         Debug.LogWarning("Hit Player");
    }
    public void DoneMelee()
    {
        meleeDoneAnimation = true;
    }
    // Jumping Logic
    private void JumpWolf(Vector2 newTarget)
    {
        Vector2 launchVelocity = Vector2.zero;
        launchVelocity.x = (newTarget.x - GetFeetPosition().x) * timeToHitTarget;    // Initial velocity in X axis
        launchVelocity.y = -(-(newTarget.y - GetFeetPosition().y) + 0.5f * Physics2D.gravity.y * timeToHitTarget * timeToHitTarget) * timeToHitTarget;

        // Add the velocity to enemy
        myRb2D.velocity = Vector2.zero; 
        myRb2D.velocity = launchVelocity;


        isJumped = true;
        myAnimator.SetTrigger("mt_Jump");
        myAnimator.ResetTrigger("mt_Fall2Idle");
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
                    if(Random.Range(0,2) == 0) // random chance to turn around
                        SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(myRb2D.position));
                    else    // Turn around
                    {
                        TurnWolfAround();
                        Vector2 platformPos = FindPlatformAbove();
                        if(platformPos == Vector2.zero) // Found no platforms, so we turn around again
                        {
                            TurnWolfAround();
                            SetNewPosTarget(groundCheckScript.platformStandingOn.GetFurtherestPosition(myRb2D.position));
                        }
                        else
                        {
                            SetNewPosTarget(platformPos);
                            JumpWolf(platformPos);
                        }
                    }

                }
                break;
        }

        // Reset Falling Bool and go back to Idle
        myAnimator.SetBool("mb_Fall", false);
        if(!isJumped)
            myAnimator.SetTrigger("mt_Fall2Idle");
    }
    // Frozen Logic
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }
    


    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        spawningZone = newSpawnZone;
        // Reset Position and Velocity
        transform.position = newPos;
        myRb2D.position = newPos;
        StopVel();
    }
    protected override void StopVel()   // Call wolf's own Animation
    {
        base.StopVel();
        // Idle Animation
        myAnimator.SetBool("mb_Move", false);
        myAnimator.SetBool("mb_Shoot", false);
        myAnimator.SetBool("mb_Melee", false);
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only accepts projectiles from players
        if (collision.gameObject.tag != "PlayerProj")
            return;

        Weapon weapon = collision.gameObject.GetComponent<Weapon>();
        AttackType type = weapon.at;
        
        // if special Ice attack
        if (type == AttackType.ICE )
        {
            // Are we already frozen?
            if (seManager.EffectAlreadyIn("SW_FrozenSE"))
                return;
            // Add new status effect
            seManager.AddEffect("SW_FrozenSE", ObjectPooler.Instance.FetchGO_Pos("SW_FrozenSE", myRb2D.position).GetComponent<BaseStatusEffect>(), this);
        }
        // if special Fire attack
        else if(type == AttackType.FIRE)
        {
            // Add new status effect
            seManager.AddEffect("SW_BurningSE", ObjectPooler.Instance.FetchGO_Pos("SW_BurningSE", myRb2D.position).GetComponent<BaseStatusEffect>(), this);
        }

        // Damage
        ModifyHealth(-weapon.GetActualDamage());


        // set attacked bool, if we are attacking the egg
        if(currentState == STATES.S_SHOOT_EGG || currentState == STATES.S_MELEE_EGG
             /*|| currentState == STATES.S_MELEE_PLAYER || currentState == STATES.S_SHOOT_PLAYER*/)
        {
            playerAttackedWolf = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Player detect range
        Gizmos.DrawLine(groundCast.position, groundCast.position + (Vector3.down * groundCastLength));
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
        // Shooting range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxShootingRange);
        // Melee Range
        Gizmos.DrawWireSphere(transform.position, meleeDistance);


        // Platform detecting
        Gizmos.color = Color.blue;
        Vector3 tempoDir = transform.right;
        tempoDir.y = 1;
        Gizmos.DrawWireCube((Vector2)transform.position + (sideTopDetect.detectOffset * tempoDir), sideTopDetect.detectSize);
        Gizmos.DrawWireCube((Vector2)transform.position + (bottomDetect.detectOffset * tempoDir), bottomDetect.detectSize);
    }
}
