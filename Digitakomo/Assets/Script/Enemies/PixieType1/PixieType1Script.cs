
using UnityEngine;

public class PixieType1Script : EnemyBaseClass
{
    // When to stop
    [Header("Stopping Data")]
    [SerializeField]
    float minimumStopInterval = 3.0f;
    [SerializeField]
    float maxStopInterval = 10.0f;
    float whenToStopTimer = 0.0f; // Used to cache how long the stop interval is
    [SerializeField]
    float howLongToStopFor = 2.0f;  // How long each stop is
    float stopTimer = 0.0f; // Countdown until we move again
    bool isStopped = false;   // Have we stopped?
    // What to shoot
    [SerializeField]
    GameObject projectilePrefab = null;

    // Used to identify which wayPoint group are we assigned to
    int waypointGroup = -1;
    int currentWaypoint = 0;


    // Start is called before the first frame
    void Awake()
    {
        Init();

        // Get how long to wait before next stop
        SetNewRandomStopTimer();
    }


    // Update is called once per frame
    void Update()
    {
        // Move
        Move();
    }

    void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0.0f)
        {
            // Attacking
            Attack();
        }
    }

    // Function to encapsulate the Moving Logic
    protected override void Move()
    {
        // if no waypoint yet, don't update
        if (waypointGroup == -1)
            return;

        // Are we stopping or moving
        if (!isStopped)
        {
            // Decrement Timer for Stopping
            whenToStopTimer -= Time.deltaTime;
            if (whenToStopTimer <= 0.0f)
                StopMovingObject_Timer();

            // Move towards using baseClass
            base.Move();

            // Check if we are reaching waypoint
            CheckNextWaypoint();
        }
        else
        {
            // Decrement Timer for how long to stop
            stopTimer -= Time.deltaTime;
            if (stopTimer < 0.0f)
            {
                isStopped = false;
                // Get how long to wait before next stop
                SetNewRandomStopTimer();
            }
        }
    }
    // Function to encapsulate the Attacking Logic
    void Attack()
    {
        GameObject go = ObjectPooler.Instance.FetchGO(projectilePrefab.name);
        go.GetComponent<Rigidbody2D>().position = transform.position;

        // reset timer
        attackTimer = attackTime;
    }


    #region WayPoints
    // Checks if we need to go to next waypoint
    void CheckNextWaypoint()
    {
        // Check if we have reached the waypoint
        if ((moveTargetPos - myRb2D.position).sqrMagnitude > 2.0f)
            return;

        // Get the next waypoint
        WaypointGroupManager.WaypointReturnData temp = WaypointGroupManager.instance.GetNextWaypoint_Wrapped(waypointGroup, currentWaypoint);
        moveTargetPos = temp.nextPosition;
        currentWaypoint = temp.nextWaypointIndex;
    }
    // Sets the object's Waypoint Group
    public void SetWaypointGroup(int newWaypointGroupID)
    {
        // cache the new waypoint group
        waypointGroup = newWaypointGroupID;

        // Get the first waypoint position
        currentWaypoint = 0;
        moveTargetPos = WaypointGroupManager.instance.GetWaypoint(waypointGroup, currentWaypoint).position;
    }
    #endregion


    // When the whenToStopMoving Timer
    // as reached 0.0, so we need to stop moving for now
    void StopMovingObject_Timer()
    {
        isStopped = true;
        // Reset timer
        stopTimer = howLongToStopFor;
    }
    // Get a new random stopping interval
    void SetNewRandomStopTimer()
    {
        whenToStopTimer = Random.Range(minimumStopInterval, maxStopInterval);
    }


    // Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only accepts projectiles from players
        if (collision.gameObject.tag != "PlayerProj")
            return;
        Weapon weapon = collision.gameObject.GetComponent<Weapon>();
        AttackType type = weapon.at;
        // if immune
        if (type == AttackType.ICE || type == AttackType.ICE_JUMP || type == AttackType.Normal)
            return;

        // One Hit Kill
        ModifyHealth(-weapon.GetActualDamage());
    }

    // Reset Function
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        // Reset Shared Data
        base.ResetEnemy(newSpawnZone, newPos);

        // Get your new waypoint here..
        SetWaypointGroup(newSpawnZone.GetComponent<SpawnZone>().GetRandomPath());
    }
}
