using UnityEngine;

public class IceCharacter : Character
{
    [Header("Weapon")]
    public IceMissile IceMissile;
    private float SpecialFireRate;

    // ice platform
    public IcePlatform icePlatform;
    private SpriteRenderer iceRender;
    private Collider2D iceCollider;

    void Start()
    {
        iceRender = icePlatform.GetComponent<SpriteRenderer>();
        iceCollider = icePlatform.GetComponent<Collider2D>();

        iceRender.enabled = false;
        iceCollider.enabled = false;

        SpecialFireRate = IceMissile.firerate;
        myRb2D = GetComponent<Rigidbody2D>();
        // Calculate how many jumps we can do
        jumpsLeft = 1 + extraJumps;
    }

    // Fixed Update called every physics Update
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (horizontalInput == 0.0f)
            StopHorizontalMovement();
        else
        {
            if (isLockMovement <= 0)
            {
                MoveHorizontal();
            }
        }
    }


    // Flip Player when changing direction
    void Flip()
    {
        // Clear the existing Velocity
        myRb2D.velocity = myRb2D.velocity * 0.4f;
    }


    // Player moving
    public override void MoveHorizontal()
    {
        // Check if we need to change direction
        if (horizontalInput > 0 && !facingRight)
        {
            facingRight = true;
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            facingRight = false;
            Flip();
        }

        // Check if we can continue to accelerate
        if (horizontalInput * myRb2D.velocity.x < maxMoveSpeed)
            myRb2D.AddForce(Vector2.right * horizontalInput * moveAcceleration * Time.deltaTime);

        // Clamp the speed within the maxMoveSpeed
        if (Mathf.Abs(myRb2D.velocity.x) > maxMoveSpeed)
            myRb2D.velocity = new Vector2(Mathf.Sign(myRb2D.velocity.x) * maxMoveSpeed, myRb2D.velocity.y);
    }

    // Player Jumping
    public override void Jump()
    {
        // No more jumps left and still not grounded
        if (jumpsLeft <= 0 && !isGrounded)
            return;
        // if ice character enable this
        else if (GetAttackType() == AttackType.ICE && jumpsLeft == 1 && !isGrounded)
        {
            CreateIcePlatform();
            jumpsLeft--;
            return;
        }
        else if (jumpsLeft >= 0 && isGrounded)  // Grounded, so reset jumps
            jumpsLeft = extraJumps + 1;

        // Jump
        myRb2D.velocity = Vector2.up * jumpAcceleration;

        // Reduce number of jumps
        jumpsLeft--;
    }

    public override void Attack()
    {
        if (NextPunch <= Time.time)
        {
            NextPunch = Time.time + PunchRate;
        }
    }

    public override void SpecialAttack()
    {
        int direction = GetAttackDirection();
        Vector3 createdPositon = GetCreatePosition();
        bool isCreated = GetIsCreated();

        // Set direction
        int xDir = 0;
        int yDir = 0;
        int rotation = 0;
        switch (direction)
        {
            case 0:
                xDir = -1;
                break;
            case 1:
                xDir = -1;
                yDir = 1;
                rotation = 135;
                break;
            case 2:
                yDir = 1;
                rotation = 90;
                break;
            case 3:
                xDir = 1;
                yDir = 1;
                rotation = 45;
                break;
            case 4:
                xDir = 1;
                break;
        }

        if (NextSpecialFire <= Time.time)
        {
            Debug.Log("Special Attack");
            GameObject go = ObjectPooler.Instance.FetchGO("IceMissile");
            IceMissile icems = go.GetComponent<IceMissile>();
            icems.Restart();
            icems.transform.position = transform.position;
            icems.SetRotation(rotation);
            icems.SetMissileDirection(xDir, yDir);
            NextSpecialFire = Time.time + SpecialFireRate;
        }

    }


    public virtual void CreateIcePlatform()
    {
        iceRender.enabled = true;
        iceCollider.enabled = true;
        icePlatform.transform.position = transform.position - new Vector3(0, 2f, 0);
    }

    private Vector3 GetCreatePosition()
    {
        return transform.position;
    }

    /**
     *     2
     *   1   3
     * 0       4
     **/
    private int GetAttackDirection()
    {
        if (!WPressed && !APressed && !DPressed)
        {
            return latestDirection;
        }
        if (WPressed)
        {
            if (APressed)
            {
                latestDirection = 1;
                return latestDirection;
            }
            else if (DPressed)
            {
                latestDirection = 3;
                return latestDirection;
            }
            else
            {
                latestDirection = 2;
                return latestDirection;
            }
        }
        else
        {
            if (APressed)
            {
                latestDirection = 0;
                return latestDirection;
            }
            else
            {
                latestDirection = 4;
                return latestDirection;
            }
        }
    }

    private bool GetIsCreated()
    {
        if (APressed && DPressed)
        {
            return false;
        }
        return true;
    }
}
