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
    public STATES currentState;
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
    float playerDetectRange = 1.0f;     // The maximum range that we can detect the player
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
    [SerializeField]        // The maximum range for melee
    float meleeRange = 1.0f;
    bool meleeDoneAnimation = false;    // Used to check if we have finished the Melee Animation
    #endregion
    #region Charging
    [SerializeField]        // The maximum range for charging
    float chargingRange = 1.0f;
    bool chargingDoneAnimation = false;    // Used to check if we have finished the Charging Animation
    #endregion
    #region Roaring
    [SerializeField]        // The maximum range for roaring
    float roaringRange = 1.0f;
    bool roaringDoneAnimation = false;    // Used to check if we have finished the Roaring Animation
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        
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

                    // check whether we can attack
                    if (CheckAttack())
                    {
                        RandomiseAttack();  // Randomise a new attacking method
                        return;
                    }
                        
                }
                break;
            case STATES.S_CHARGE:
                {
                    if (chargingDoneAnimation)
                        currentState = STATES.S_WALK;
                }
                break;
            case STATES.S_SHOOT:
                {
                    if (shootingDoneAnimation)
                        currentState = STATES.S_WALK;
                }
                break;
            case STATES.S_MELEE:
                {
                    if (meleeDoneAnimation)
                        currentState = STATES.S_WALK;
                }
                break;
            case STATES.S_ROAR:
                {
                    if (roaringDoneAnimation)
                        currentState = STATES.S_WALK;
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
        result = Physics2D.OverlapCircle(myRb2D.position, playerDetectRange, LayerMask.GetMask("Player"));
        if (result == null)
            return null;
        if (result.gameObject.tag != "Player")
            return null;
        // check are we actually in range or we just hit the collider
        if (((Vector2)result.gameObject.transform.position - myRb2D.position).sqrMagnitude > (playerDetectRange * playerDetectRange))
            return null;

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
    public void Melee()
    {

    }
    public void DoneMelee()
    {
        meleeDoneAnimation = true;
    }
    // Charging Logic
    public void Charge()
    {

    }
    public void DoneCharge()
    {
        chargingDoneAnimation = true;
    }
    // Roaring Logic
    public void Roar()
    {

    }
    public void DoneRoar()
    {
        chargingDoneAnimation = true;
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
                    if (distance < (meleeRange * meleeRange))
                    {
                        currentState = STATES.S_MELEE;
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
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        // Shooting range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxShootingRange);
        // Ground Cast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCast.position, groundCast.position + (Vector3.down * groundCastLength));
    }
}
