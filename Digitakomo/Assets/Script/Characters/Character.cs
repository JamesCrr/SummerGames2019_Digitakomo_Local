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
    public float HP = 500;
    public float Energy = 200;

    // Attack
    protected bool IsAttacking = false;
    protected bool IsSpecialAttacking = false;
    protected Collider2D AttackCollider;

    // Animation
    private Animator Animate;

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
    }

    protected virtual void Update()
    {
        // Get latest Horizontal Input from player
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (horizontalInput == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalInput == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Increase gravity if player has jumped and is currently falling
        if (myRb2D.velocity.y < 0)
            myRb2D.velocity += Vector2.up * (Physics2D.gravity.y * fallingMultiplyer) * Time.deltaTime;
        // If player wants to jump
        if (Input.GetButtonDown("Jump"))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isLockMovement += 1;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isLockMovement -= 1;
        }

        Animate.SetBool("Attacking", IsAttacking);
    }

    protected virtual void FixedUpdate()
    {
        CheckGrounded();
        if (IsAttacking)
            Attack();
        if (IsSpecialAttacking)
            SpecialAttack();
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
        return energyToUse <= Energy;
    }

    public float UseEnergy(float energyToUse)
    {
        if (!IsEnergyLeft(energyToUse))
        {
            throw new ArgumentException("eneryToUse must be more than energy, check to by IsEnergyLeftFirst");
        }
        Energy -= energyToUse;
        return Energy;
    }
}
