using UnityEngine;

public class FireCharacter : Character
{
    [Header("Weapon")]
    public FlameProjectile flameThrower;
    private float SpecialFireRate;

    // Start is called before the first frame update
    void Start()
    {
        SpecialFireRate = flameThrower.firerate;
        enerygyPerSpecialAttack = flameThrower.enerygyPerSpecialAttack;
    }

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
        {
            myRb2D.AddForce(Vector2.right * horizontalInput * moveAcceleration * Time.deltaTime);
        }

        // Clamp the speed within the maxMoveSpeed
        if (Mathf.Abs(myRb2D.velocity.x) > maxMoveSpeed)
            myRb2D.velocity = new Vector2(Mathf.Sign(myRb2D.velocity.x) * maxMoveSpeed, myRb2D.velocity.y);
    }

    public override void Jump()
    {
        // No more jumps left and still not grounded
        if (jumpsLeft <= 0 && !isGrounded)
            return;
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
            if (!IsHasEnoughEnergy(enerygyPerSpecialAttack))
            {
                return;
            }

            ReduceEnergy(enerygyPerSpecialAttack);
            GameObject go = ObjectPooler.Instance.FetchGO("FireProjectile");
            FlameProjectile firep = go.GetComponent<FlameProjectile>();
            firep.Restart();
            firep.transform.position = transform.position;
            firep.SetRotation(rotation);
            firep.SetMissileDirection(xDir, yDir);
            NextSpecialFire = Time.time + SpecialFireRate;
        }

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
