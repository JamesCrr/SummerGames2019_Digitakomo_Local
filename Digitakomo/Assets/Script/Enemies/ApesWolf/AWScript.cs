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
    [SerializeField]
    // Maximum Attack Range
    float maxShootingRange = 5.0f;
    [SerializeField]
    float timeToHitTarget = 1.0f;   // How long for the projectile to hit smth
    bool shootingDoneAnimation = false;    // Used to check if we have finished the Shooting Animation
    #endregion
    #region Melee

    bool meleeDoneAnimation = false;    // Used to check if we have finished the Melee Animation
    #endregion
    #region Charging

    bool chargingDoneAnimation = false;    // Used to check if we have finished the Charging Animation
    #endregion
    #region Roaring
    
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

                }
                break;
            case STATES.S_CHARGE:
                {

                }
                break;
            case STATES.S_SHOOT:
                {

                }
                break;
            case STATES.S_MELEE:
                {
                    
                }
                break;
            case STATES.S_ROAR:
                {

                }
                break;
        }
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
