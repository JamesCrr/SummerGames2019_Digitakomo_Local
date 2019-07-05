using UnityEngine;

public class IceCharacter : Character
{
    [Header("Weapon")]
    public IceMissile IceMissile;
    private float SpecialFireRate;

    // ice platform
    public IcePlatform icePlatform;

    void Start()
    {
        icePlatform = Instantiate(icePlatform);

        SpecialFireRate = IceMissile.firerate;
        enerygyPerSpecialAttack = IceMissile.enerygyPerSpecialAttack;
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
        GetAttackDirection();
    }



    // Player Jumping
    public override void Jump()
    {
        base.Jump();
        // No more jumps left and still not grounded
        if (jumpsLeft <= 0 && !isGrounded)
            return;
        // if ice character enable this
        else if (GetAttackType() == AttackType.ICE && jumpsLeft == 2 && !isGrounded)
        {
            myRb2D.velocity = new Vector2(myRb2D.velocity.x, 0);
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
            if (!IsHasEnoughEnergy(enerygyPerSpecialAttack))
            {
                return;
            }

            ReduceEnergy(enerygyPerSpecialAttack);
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
        icePlatform.Restart(transform.position - new Vector3(0, 2f, 0));
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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.GetComponent<IcePlatform>() != null)
        {
            myRb2D.velocity = currentVelocity;
        }
    }
}
