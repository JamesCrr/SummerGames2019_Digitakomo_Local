using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Which Character are we using
    //[System.Serializable]
    //public class PlayerType_Template
    //{
    //    // The Animator to use for this character
    //    public RuntimeAnimatorController characterAnimator = null;
    //    // The Sprite to use for this character
    //    public Sprite characterSprite = null;
    //}
    //// Used to store the options for both states
    //[SerializeField]
    //PlayerType_Template iceTemplate;
    // What type are we using
    [SerializeField]
    protected AttackType SelectedType = AttackType.UNKNOWN;


    // Data that all characters share
    [Header("Common Data shared by all characters")]
    [SerializeField]
    protected float moveAcceleration = 700.0f;    // How fast does the character move
    [SerializeField]
    protected float maxMoveSpeed = 5.0f;      // The max speed the player can move
    public float horizontalInput = 0.0f;   // Used to cache the horizontal input
    protected bool facingRight = false;  // Bool to check if we are facing right or left
    public int isLockMovement = 0;      // 0 is run movement
    // Jumping related
    [SerializeField]
    protected float jumpAcceleration = 10.0f;      // How much force does the character use to jump
    public bool isGrounded = true;      // Whether this character is grounded
    [SerializeField]
    Transform groundCheck = null;       // Where to start detecting is ground
    [SerializeField]
    float groundCheckRadius = 0.5f;     // The radius to detect the ground
    [SerializeField]
    LayerMask whatIsGround;     // What layer to detect as ground
    [SerializeField]
    protected int extraJumps = 1;       // How many extra jumps we get, not including default
    protected int jumpsLeft = 1;
    [SerializeField]
    protected float fallingMultiplyer = 2.0f;   // How much fast should this character fall

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;

    // health and energy
    public float MaxHP = 500;
    private float HP;
    public float MaxEnergy = 200;
    private float MP;

    // Attack
    protected bool IsAttacking = false;
    protected bool IsSpecialAttacking = false;
    protected Collider2D AttackCollider;
    public float PunchRate = 0.1f;
    protected float NextPunch;
    protected bool WPressed = false;
    protected bool APressed = false;
    protected bool DPressed = false;
    protected int latestDirection = 4;
    protected float NextSpecialFire;
    public float enerygyPerSpecialAttack = 20;

    // Animation
    private Animator Animate;

    public int player = 1;



    // Start is called before the first frame update
    void Awake()
    {
        Collider2D[] colliders = (GetComponentsInChildren<BoxCollider2D>());
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "AttackCollider")
                AttackCollider = collider;
        }

        myRb2D = GetComponent<Rigidbody2D>();
        Animate = GetComponent<Animator>();

        HP = MaxHP;
        MP = MaxEnergy;
    }

    protected virtual void Update()
    {
        HandleInput();
        // Increase gravity if player has jumped and is currently falling
        if (myRb2D.velocity.y < 0)
            myRb2D.velocity += Vector2.up * (Physics2D.gravity.y * fallingMultiplyer) * Time.deltaTime;
        Animate.SetBool("Attacking", IsAttacking);
    }

    protected virtual void FixedUpdate()
    {
        CheckGrounded();
        if (IsAttacking)
            Attack();
        if (IsSpecialAttacking)
            SpecialAttack();

        if (MP < MaxEnergy)
        {
            MP += 0.5f;
        }
    }

    // Moving Horizontal, left and right
    public virtual void MoveHorizontal()
    {
        throw new NotImplementedException();
    }
    // Jumping
    public virtual void Jump()
    {
        throw new NotImplementedException();
    }

    // Attack (punch)
    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    // Special Attack
    public virtual void SpecialAttack()
    {
        throw new NotImplementedException();
    }

    protected bool IsHasEnoughEnergy(float energy)
    {
        return energy <= MP;
    }

    protected float ReduceEnergy(float energy)
    {
        if (!IsHasEnoughEnergy(energy))
        {
            throw new ArgumentException("Call Is has enough energy before reduce!!");
        }
        MP -= energy;
        return MP;
    }

    // Check if we are grounded, are we touching the ground?
    protected void CheckGrounded()
    {
        // Check if we are grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        Debug.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x + groundCheckRadius, groundCheck.position.y + groundCheckRadius, 1.0f), Color.red);
    }
    // Stop all horizontal Velocities of the rigidBody
    protected void StopHorizontalMovement()
    {
        myRb2D.velocity = new Vector2(0.0f, myRb2D.velocity.y);
    }
    // Stop all Vertical Velocities of the rigidBody
    protected void StopVerticalMovement()
    {
        myRb2D.velocity = new Vector2(myRb2D.velocity.x, 0.0f);
    }

    // Returns the currently selected type
    public AttackType GetAttackType()
    {
        if (SelectedType == AttackType.UNKNOWN)
        {
            throw new NotImplementedException();
        }
        return SelectedType;
    }

    public float TakeDamage(float damage)
    {
        HP -= damage;
        return HP;
    }

    public bool IsEnergyLeft(float energyToUse)
    {
        return energyToUse <= MP;
    }

    public float UseEnergy(float energyToUse)
    {
        if (!IsEnergyLeft(energyToUse))
        {
            throw new ArgumentException("eneryToUse must be more than energy, check to by IsEnergyLeftFirst");
        }
        MP -= energyToUse;
        return MP;
    }

    public float GetCurrentHP()
    {
        return HP;
    }

    public float GetCurrentMP()
    {
        return MP;
    }

    private void HandleInput()
    {
        float moveLeft = Input.GetAxisRaw("Player" + player + "MoveLeft");
        float moveRight = Input.GetAxisRaw("Player" + player + "MoveRight");
        float defense = Input.GetAxisRaw("Player" + player + "Defense");
        float attack = Input.GetAxisRaw("Player" + player + "Attack");
        float specialAttack = Input.GetAxisRaw("Player" + player + "SpecialAttack");
        float lookUp = Input.GetAxisRaw("Player" + player + "LookUp");
        bool jump = Input.GetButtonDown("Player" + player + "Jump");
        bool lockMovementDown = Input.GetButtonDown("Player" + player + "LockMovement");
        bool lockMovementUp = Input.GetButtonUp("Player" + player + "LockMovement");

        // Get latest Horizontal Input from player
        horizontalInput = moveRight - moveLeft;


        if (horizontalInput == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalInput == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Animate.SetBool("Walking", horizontalInput == 0);


        // If player wants to jump
        if (jump)
        {
            Animate.SetTrigger("Jump");
            Jump();
        }

        if (lockMovementDown)
        {
            isLockMovement += 1;
        }
        if (lockMovementUp)
        {
            isLockMovement -= 1;
        }

        if (Input.GetButtonDown("Player" + player + "LookUp"))
        {
            WPressed = true;
        }
        else if (Input.GetButtonUp("Player" + player + "LookUp"))
        {
            WPressed = false;
        }

        if (Input.GetButtonDown("Player" + player + "MoveLeft"))
        {
            APressed = true;
        }
        else if (Input.GetButtonUp("Player" + player + "MoveLeft"))
        {
            APressed = false;
        }
        if (Input.GetButtonDown("Player" + player + "MoveRight"))
        {
            DPressed = true;
        }
        else if (Input.GetButtonUp("Player" + player + "MoveRight"))
        {
            DPressed = false;
        }

        if (Input.GetButtonDown("Player" + player + "Attack"))
        {
            IsAttacking = true;
            NextPunch = Time.time;
            AttackCollider.enabled = true;
        }
        else if (Input.GetButtonUp("Player" + player + "Attack"))
        {
            IsAttacking = false;
            AttackCollider.enabled = false;
        }
        if (Input.GetButtonDown("Player" + player + "SpecialAttack"))
        {
            IsSpecialAttacking = true;
            NextSpecialFire = Time.time;
        }
        else if (Input.GetButtonUp("Player" + player + "SpecialAttack"))
        {
            IsSpecialAttacking = false;
        }
    }
}
