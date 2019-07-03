using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWScript : EnemyBaseClass
{
    #region Classes and Enums
    // States for StateMachine
    public enum STATES
    {
        S_WALK,
        S_CHARGE,
        S_SHOOT,
        S_MELEE,
        S_ROAR,
    }
    public STATES currentState = STATES.S_WALK;
    // Attack Method
    public enum ATTACK
    {
        A_CHARGE,
        A_SHOOT,
        A_MELEE,
        A_ROAR
    }
    public ATTACK currentAttack = ATTACK.A_CHARGE;
    #endregion
    [Header("Ape Wolf Class")]
    [SerializeField]
    Vector2 detectSize = new Vector2(5, 2);     // The maximum range that we can detect the player
    GameObject targetObject = null;     // Used to store the Targeted Object, can be player or egg
    Collider2D result = null;       // To store the overlap circle cast
    #region GroundCasting
    [Header("GroundCasting")]
    [SerializeField]
    // Ground Cast
    Transform groundCast = null;
    [SerializeField]
    float groundCastLength = 0.09f;
    RaycastHit2D rayhit2D = new RaycastHit2D();
    #endregion
    #region Shooting
    [Header("Shooting")]
    // What to shoot
    [SerializeField]
    GameObject projectilePrefab = null;
    [SerializeField]
    Transform shootingPos = null;
    [SerializeField]        // Maximum Attack Range
    float maxShootingRange = 5.0f;
    [SerializeField]
    float timeToHitTarget = 1.0f;   // How long for the projectile to hit smth
    bool shootingDoneAnimation = false;    // Used to check if we have finished the Shooting Animation
    #endregion
    #region Melee
    [Header("Melee")]
    //[SerializeField]        // How much to offset the hitbox for melee
    //Vector2 meleeBoxOffset = Vector2.zero;
    //[SerializeField]        // The size of the hitbox
    //Vector2 meleeBoxSize = Vector2.zero;
    //Bounds meleeHitBox = new Bounds();
    [SerializeField]         // The maximum range for melee
    float meleeAttackRange = 1.0f;      
    bool meleeDoneAnimation = false;    // Used to check if we have finished the Melee Animation
    #endregion
    #region Charging
    [Header("Charging")]
    [SerializeField]        // The maximum range for charging
    float chargingRange = 1.0f;
    bool startCharge = false;       // Have we started charging
    [SerializeField]
    float chargingSpeed = 4.0f;     // The speed we charge at
    #endregion
    #region Roaring
    [Header("Roaring")]
    [SerializeField]        // The maximum range for roaring
    float roaringRange = 1.0f;
    bool roaringDoneAnimation = false;    // Used to check if we have finished the Roaring Animation
    #endregion



    // Awake 
    void Awake()
    {
        base.Init();

        // Calculate the percentage difference to scale the colliders
        Vector2 percentageDifference = Vector2.zero;
        percentageDifference.x = transform.localScale.x / 1.0f;
        percentageDifference.y = transform.localScale.y / 1.0f;
        // Multiply ranges with scale
        // Platform detecting
        detectSize.x *= percentageDifference.x;
        detectSize.y *= percentageDifference.y;
        // Charging 
        chargingRange *= percentageDifference.x;
        // roaring
        roaringRange *= percentageDifference.x;
        // Shooting
        maxShootingRange *= percentageDifference.x;
        // Melee
        meleeAttackRange *= percentageDifference.x;


        // Set up the melee hitbox
        //meleeHitBox.center = transform.position + (Vector3)(meleeBoxOffset * transform.right);
        //meleeHitBox.size = meleeBoxSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
    }


    // Update miniature State Machine
    void UpdateStates()
    {
        switch (currentState)
        {
            case STATES.S_WALK:
                {
                    // Is a player in range?
                    targetObject = IsPlayerInRange();
                    if (targetObject == null)       // If not, set the egg as the target
                        SetNewTargetObject(Egg.Instance.gameObject);
                    else
                        SetNewTargetObject(targetObject);


                    // check whether we can attack
                    if (CheckAttack())
                    {
                        //RandomiseAttack();  // Randomise a new attacking method
                        return;
                    }

                    // Move towards target
                    if(!MoveApe())
                    {
                        // set to idle
                        StopApe();
                        return;
                    }
                    // set to moving
                    myAnimator.SetBool("mb_Move", true);
                }
                break;
            case STATES.S_CHARGE:
                {
                    // Wait for animation
                    if (!startCharge)
                        return;

                    // Charge towards the target
                    if(!ChargeApe())  // if we can't move anymore, then go back to egg
                    {
                        StopCharge();
                        return;
                    }

                    // check distance, if reached destiantion, stop charge
                    if (Mathf.Abs((moveTargetPos.x - myRb2D.position.x)) < 1.0f)
                        StopCharge();
                }
                break;
            case STATES.S_SHOOT:
                {
                    if (shootingDoneAnimation)
                    {
                        currentState = STATES.S_WALK;
                        shootingDoneAnimation = false;  
                    }
                        
                }
                break;
            case STATES.S_MELEE:
                {
                    if (meleeDoneAnimation)
                    {
                        currentState = STATES.S_WALK;
                        meleeDoneAnimation = false;
                    }
                }
                break;
            case STATES.S_ROAR:
                {
                    if (roaringDoneAnimation)
                    {
                        currentState = STATES.S_WALK;
                        roaringDoneAnimation = false;   
                    }
                }
                break;
        }
    }



    // Function to move and return whether we can continue to move
    bool MoveApe()
    {
        FlipEnemy();

        // Cast below us
        // Check if we can even move
        if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength), LayerMask.GetMask("Ground")))
        {
            // Move
            myRb2D.MovePosition(myRb2D.position + (moveDirection * moveSpeed * Time.deltaTime));
            return true;
        }

        return false;
    }
    // Function to charge the ape
    bool ChargeApe()
    {
        // Cast below us
        // Check if we can even move
        if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength), LayerMask.GetMask("Ground")))
        {
            // Move
            myRb2D.MovePosition(myRb2D.position + (moveDirection * chargingSpeed * Time.deltaTime));
            return true;
        }

        return false;
    }
    void StopApe()
    {
        myRb2D.velocity = Vector2.zero;
        myAnimator.SetBool("mb_Move", false);
        myAnimator.SetBool("mb_Charge", false);
        myAnimator.SetBool("mb_Roar", false);
        myAnimator.SetBool("mb_Melee", false);
        myAnimator.SetBool("mb_Shoot", false);
    }
    // Returns the player Object if he is in Range
    // Player must be in Player Layer and Tag
    // Does not count EGG as player
    GameObject IsPlayerInRange()
    {
        // Get if we have hit anything, player or egg
        result = Physics2D.OverlapBox(myRb2D.position, detectSize, 0.0f, LayerMask.GetMask("Player"));
        if (result == null)
            return null;
        if (result.gameObject.tag != "Player")
            return null;
        // check are we actually in range or we just hit the collider
        if (((Vector2)result.gameObject.transform.position - myRb2D.position).sqrMagnitude > (detectSize.x * detectSize.x))
            return null;
        // raycast to check if we can actually go to player
        Vector2 testDirection = (Vector2)result.gameObject.transform.position - myRb2D.position;
        rayhit2D = Physics2D.Raycast(shootingPos.position, testDirection.normalized, 5, LayerMask.GetMask("Ground"));
        if (rayhit2D.collider != null)  // If we hit smth, return null
        {
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * 5, Color.yellow);
            return null;
        }
        else
            Debug.DrawLine(shootingPos.position, (Vector2)shootingPos.position + testDirection.normalized * 5, Color.red);

        return result.gameObject;
    }

    // Set new target for direction
    public void SetNewTarget(Vector2 newTarget)
    {
        moveTargetPos = newTarget;
        moveDirection = (newTarget - myRb2D.position).normalized;
    }
    // Set new target object as target
    public void SetNewTargetObject(GameObject newtargetObj)
    {
        targetObject = newtargetObj;
        SetNewTarget(newtargetObj.transform.position);
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
    public void DoneMelee()
    {
        meleeDoneAnimation = true;
    }
    // Charging Logic
    public void StartCharge()
    {
        startCharge = true;

        // set new target
        Vector2 newtarget = targetObject.transform.position;
        Vector2 direction = newtarget - myRb2D.position;
        newtarget += direction * 1.5f;
        SetNewTarget(newtarget);
    }
    void StopCharge()
    {
        currentState = STATES.S_WALK;
        startCharge = false;
        // go back to idle
        myAnimator.SetBool("mb_Charge", false);
    }
    // Roaring Logic
    public void Roar()
    {

    }
    public void DoneRoar()
    {
        roaringDoneAnimation = true;
    }
    // Function to encapsulate all of the attack detection
    // Returns true if a change in state was made
    // Returns false if no change in state was made
    bool CheckAttack()
    {
        float distance = ((Vector2)targetObject.transform.position - myRb2D.position).sqrMagnitude;

        switch (currentAttack)
        {
            case ATTACK.A_CHARGE:
                {
                    // Can we charge?
                    if(distance < (chargingRange * chargingRange))
                    {
                        currentState = STATES.S_CHARGE;
                        // set to charge ani
                        StopApe();
                        myAnimator.SetBool("mb_Charge", true);
                        return true;
                    }
                }
                break;
            case ATTACK.A_SHOOT:
                {
                    // Can we shoot?
                    if (distance < (maxShootingRange * maxShootingRange))
                    {
                        currentState = STATES.S_SHOOT;
                        return true;
                    }
                }
                break;
            case ATTACK.A_MELEE:
                {
                    // Can we melee?
                    if (distance < (meleeAttackRange*meleeAttackRange))
                    {
                        currentState = STATES.S_MELEE;
                        // set to charge ani
                        StopApe();
                        myAnimator.SetBool("mb_Melee", true);
                        return true;
                    }
                }
                break;
            case ATTACK.A_ROAR:
                {
                    // Can we roar?
                    if (distance < (roaringRange * roaringRange))
                    {
                        currentState = STATES.S_ROAR;
                        return true;
                    }
                }
                break;
        }

        return false;
    }
    // Randomise attacking method
    void RandomiseAttack()
    {
        currentAttack = (ATTACK)Random.Range((int)ATTACK.A_CHARGE, (int)ATTACK.A_ROAR+1);
    }

    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        base.ResetEnemy(newSpawnZone, newPos);

    }
    #endregion



    private void OnDrawGizmos()
    {
        // Player Detect Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, detectSize);
        // Shooting range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxShootingRange);
        // Melee range
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
        // Charging range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chargingRange);
        // Roaring range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roaringRange);
        // Ground Cast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCast.position, groundCast.position + (Vector3.down * groundCastLength));


        // Melee box
        //Vector2 newRight = transform.right;
        //newRight.y = 1;
        //meleeHitBox.center = transform.position + (Vector3)(meleeBoxOffset * newRight);
        //meleeHitBox.size = meleeBoxSize;
        //Gizmos.DrawWireCube(meleeHitBox.center, meleeHitBox.size);
    }

   
}
