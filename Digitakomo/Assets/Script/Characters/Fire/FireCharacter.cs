﻿using UnityEngine;

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

        // find malee attack time base on animation
        AnimationClip[] clips = Animate.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Fire_MaleeAttack")
            {
                maleeAttackClipTime = clip.length;
            }
            if (clip.name == "Fire_SpecialAttack")
            {
                specialAttackClipTime = clip.length;
            }
        }
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

    // Player Moving
    // Override because need to slow down when in the air
    public override void MoveHorizontal()
    {
        base.MoveHorizontal();
    }
    public override void Jump()
    {
        base.Jump();
        // No more jumps left and still not grounded
        if (jumpsLeft <= 0 && !isGrounded)
            return;
        else if (jumpsLeft >= 0 && isGrounded)  // Grounded, so reset jumps
            jumpsLeft = extraJumps + 1;

        if (GetAttackType() == AttackType.FIRE && jumpsLeft == 1)
        {
            Animate.SetTrigger("specialJump");
            Animate.SetBool("isFalling", false);
            // GetComponentInChildren<FireRocket>().SetEnabled(true);
        }
        // Jump
        Vector2 tempJump = myRb2D.velocity;
        tempJump.x *= 0.8f;
        tempJump.y = Vector2.up.y * jumpAcceleration;
        myRb2D.velocity = tempJump;

        // Reduce number of jumps
        jumpsLeft--;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void DoneAttack()
    {
        base.DoneAttack();
    }

    public override void SpecialAttack()
    {
        base.SpecialAttack();
        // Animate.SetBool("f_SpecialAttack", true);
    }

    protected override void DoneSpecialAttack()
    {
        base.DoneSpecialAttack();
        // Animate.SetBool("f_SpecialAttack", false);
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
        GameObject go = ObjectPooler.Instance.FetchGO("FireProjectile");
        FlameProjectile firep = go.GetComponent<FlameProjectile>();
        firep.Restart();
        firep.transform.position = createdPositon;
        firep.SetRotation(rotation);
        firep.SetMissileDirection(xDir, yDir);
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
            jumpsLeft = extraJumps + 1;
        }
    }
}
