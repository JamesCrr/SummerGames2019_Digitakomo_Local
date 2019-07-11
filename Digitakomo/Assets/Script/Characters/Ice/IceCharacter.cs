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

        // find malee attack time base on animation
        AnimationClip[] clips = Animate.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Ice_MaleeAttack")
            {
                maleeAttackClipTime = clip.length;
            }
            if (clip.name == "Ice_SpecialAttack")
            {
                specialAttackClipTime = clip.length;
            }
        }
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



    protected override void Attack()
    {
        base.Attack();
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
    }

    public void Shoot()
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

        if (!IsHasEnoughEnergy(enerygyPerSpecialAttack))
        {
            return;
        }

        ReduceEnergy(enerygyPerSpecialAttack);
        GameObject go = ObjectPooler.Instance.FetchGO("IceMissile");
        IceMissile icems = go.GetComponent<IceMissile>();
        icems.Restart();
        icems.transform.position = createdPositon;
        icems.SetRotation(rotation);
        icems.SetMissileDirection(xDir, yDir);
    }


    public virtual void CreateIcePlatform()
    {
        icePlatform.gameObject.SetActive(true);
        icePlatform.Restart(transform.position - new Vector3(0, 2f, 0));
    }

    private Vector3 GetCreatePosition()
    {
        return specialAttackPosition.position;
    }

    /**
     *     2
     *   1   3
     * 0       4
     **/
    private int GetAttackDirection()
    {
        if (InputManager.GetAxisRaw("Player" + player + "MoveLeft") == 0
            && InputManager.GetAxisRaw("Player" + player + "MoveRight") == 0
            && InputManager.GetAxisRaw("Player" + player + "LookUp") == 0)
        {
            return latestDirection;
        }
        if (InputManager.GetAxisRaw("Player" + player + "MoveLeft") == 0
            && InputManager.GetAxisRaw("Player" + player + "MoveRight") == 0
            && InputManager.GetAxisRaw("Player" + player + "LookUp") == 1)
        {
            // up
            latestDirection = 2;
            return 2;
        }
        if (specialAttackPosition.position.x > transform.position.x)
        {
            // right
            if (InputManager.GetAxisRaw("Player" + player + "LookUp") == 1)
            {
                // upright
                latestDirection = 3;
                return 3;
            }
            else
            {
                latestDirection = 4;
                return 4;
            }
        }
        else
        {
            // left
            if (InputManager.GetAxisRaw("Player" + player + "LookUp") == 1)
            {
                // upleft
                latestDirection = 1;
                return 1;
            }
            else
            {
                latestDirection = 0;
                return 0;
            }
        }
    }

    private bool GetIsCreated()
    {
        if (InputManager.GetAxisRaw("Player" + player + "MoveLeft") == 1
            && InputManager.GetAxisRaw("Player" + player + "MoveRight") == 1)
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
