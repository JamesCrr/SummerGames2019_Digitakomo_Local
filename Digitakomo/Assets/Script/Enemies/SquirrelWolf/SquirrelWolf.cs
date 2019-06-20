using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    // States
    enum STATES
    {
        S_WALKTOEGG,
        S_WALKTOPLAYER,
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
                    targetPosition = Egg.Instance.transform.position;

                    // Move enemy
                    Move();
                }
                break;
            case STATES.S_WALKTOPLAYER:
                {

                }
                break;
            case STATES.S_MELEE:
                {

                }
                break;
            case STATES.S_SHOOT:
                {

                }
                break;
        }
    }

    // Returns the player Object if he is in Range
    GameObject IsPlayerInRange()
    {
        // Get if we have hit anything, player or egg
        result = Physics2D.OverlapCircle(transform.position, playerDetectionRange, LayerMask.NameToLayer("Player"));
        if (result == null)
            return null;
        if (result.gameObject.tag != "Player")
            return null;

        return result.gameObject;
    }


    protected override void Move()
    {
        // set the new direction
        direction = targetPosition - myRb2D.position;
        // Check if need to flip enemy
        FlipEnemy();

        Debug.DrawLine(groundCast.position, groundCast.position + (Vector3.down * groundCastLength), Color.red);
        // Check if we can even move
        if (Physics2D.Linecast(groundCast.position, groundCast.position + (Vector3.down * groundCastLength)))
        {
            // Move
            myRb2D.MovePosition(myRb2D.position + (direction.normalized * moveSpeed * Time.deltaTime));
        }
    }
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        
    }
}
