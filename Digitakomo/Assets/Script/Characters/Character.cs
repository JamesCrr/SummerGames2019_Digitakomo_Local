using System;
using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    // What type are we using
    [SerializeField]
    protected AttackType SelectedType = AttackType.UNKNOWN;

    // Data that all characters share
    [Header("Common Data shared by all characters")]
    [SerializeField]
    protected float initialMoveSpeed = 1f; // how the character move at first frame
    [SerializeField]
    protected float moveAcceleration = 700.0f;    // How fast does the character move
    [SerializeField]
    protected float maxMoveSpeed = 5.0f;      // The max speed the player can move
    public float horizontalInput = 0.0f;   // Used to cache the horizontal input
    protected bool facingRight = false;  // Bool to check if we are facing right or left
    public int isLockMovement = 0;      // 0 is lock movement
    // Jumping related
    [SerializeField]
    protected float jumpAcceleration = 10.0f;      // How much force does the character use to jump
    public bool isGrounded = true;      // Whether this character is grounded
    private Vector3 localScale;
    [SerializeField]
    Transform groundCheck = null;       // Where to start detecting is ground
    [SerializeField]
    float groundCheckRadius = 0.5f;     // The radius to detect the ground
    [SerializeField]
    LayerMask whatIsGround = 0;     // What layer to detect as ground
    [SerializeField]
    protected int extraJumps = 1;       // How many extra jumps we get, not including default
    protected int jumpsLeft = 1;
    [SerializeField]
    protected float fallingMultiplyer = 2.0f;   // How much fast should this character fall

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;

    [Header("HP and Energy")]
    // health and energy
    public float MaxHP = 500;
    private float HP;
    public float MaxEnergy = 200;
    private float MP;

    // Attack
    [Header("Attack")]
    protected bool isAttacking = false;
    protected bool isSpecialAttacking = false;
    protected float nextPunchTime;
    // set in child because of different clip each character
    protected float maleeAttackClipTime;
    public bool electricAttack = false;
    protected float enerygyPerSpecialAttack;
    protected Collider2D AttackCollider;
    protected int latestDirection = 4;
    protected float nextSpecialFire;
    protected float specialAttackClipTime;
    public Transform specialAttackPosition;

    // Animation
    [HideInInspector]
    public Animator Animate;

    public int player = 1;

    // Current velocity
    protected Vector2 currentVelocity;

    public Material electricEffect;
    private Material defaultMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        // initialize the keybinding
        // InputManager.Initialize(player);

        // get the attack collider
        Collider2D[] colliders = (GetComponentsInChildren<BoxCollider2D>());
        foreach (Collider2D collider in colliders)
        {
            if (collider.name == "AttackCollider")
            {
                AttackCollider = collider;
            }
        }

        // get the rigidbody
        myRb2D = GetComponent<Rigidbody2D>();

        // get the animation
        Animate = GetComponent<Animator>();

        // save the default material
        defaultMaterial = GetComponent<Renderer>().material;

        // get scale for flip
        localScale = transform.localScale;
        // flip because of sprite
        Flip();

        // set hp, energy to max hp
        HP = MaxHP;
        MP = MaxEnergy;
    }

    protected virtual void Update()
    {
        // handle all posible input from player
        HandleInput();

        // Increase gravity if player has jumped and is currently falling
        if (myRb2D.velocity.y < 0)
            myRb2D.velocity += Vector2.up * (Physics2D.gravity.y * fallingMultiplyer) * Time.deltaTime;

        // handle HP if character dead
        HandleHP();

        // get latest velocity
        currentVelocity = myRb2D.velocity;

        //do the attack
        if (isAttacking)
        {
            if (Time.time >= nextPunchTime)
            {
                Animate.SetTrigger("maleeAttack");
                nextPunchTime = Time.time + maleeAttackClipTime;
            }
        }

        if (isSpecialAttacking)
        {
            if (Time.time >= nextSpecialFire)
            {
                Animate.SetTrigger("specialAttack");
                nextSpecialFire = Time.time + specialAttackClipTime;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        // check is it ground ?
        CheckGrounded();

        handleMovementAnimation();
    }

    // Moving Horizontal, left and right
    public virtual void MoveHorizontal()
    {
        if (horizontalInput * myRb2D.velocity.x < initialMoveSpeed)
        {
            myRb2D.velocity = new Vector2(initialMoveSpeed * horizontalInput, myRb2D.velocity.y);
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

    // Flip Player when changing direction
    protected void Flip()
    {
        // Clear the existing Velocity
        myRb2D.velocity = new Vector2(myRb2D.velocity.x * 0.4f, myRb2D.velocity.y);
        // localScale.x *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        // transform.localScale = localScale;
    }

    // Jumping
    public virtual void Jump()
    {

    }

    // Attack (punch)
    protected virtual void Attack()
    {
        isAttacking = true;
        nextPunchTime = Time.time;
    }

    protected virtual void DoneAttack()
    {
        isAttacking = false;
        Animate.SetTrigger("doneMaleeAttack");
    }

    // Special Attack
    public virtual void SpecialAttack()
    {
        isSpecialAttacking = true;
        nextSpecialFire = Time.time;
    }

    protected virtual void DoneSpecialAttack()
    {
        isSpecialAttacking = false;
        Animate.SetTrigger("doneSpecialAttack");
    }

    // Set if player has enough energy before using special attack
    protected bool IsHasEnoughEnergy(float energy)
    {
        return energy <= MP;
    }

    // Reduce the Current energy by param energy
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

    public void TakeDamage(float damage)
    {
        HP -= damage;
        // FloatingTextController.CreateFloatingText(damage.ToString("F0"), transform.position);
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
        float moveLeft = InputManager.GetAxisRaw("Player" + player + "MoveLeft");
        float moveRight = InputManager.GetAxisRaw("Player" + player + "MoveRight");
        bool jump = InputManager.GetButtonDown("Player" + player + "Jump");
        bool lockMovementDown = InputManager.GetButtonDown("Player" + player + "LockMovement");
        bool lockMovementUp = InputManager.GetButtonUp("Player" + player + "LockMovement");
        bool lookUpPressed = InputManager.GetButtonDown("Player" + player + "LookUp");
        bool lookUpReleased = InputManager.GetButtonUp("Player" + player + "LookUp");

        // Get latest Horizontal Input from player
        horizontalInput = moveRight - moveLeft;

        // If player wants to jump
        if (jump)
        {
            Jump();
        }

        if (lockMovementDown)
        {
            isLockMovement += 1;
            myRb2D.velocity = new Vector2(0, myRb2D.velocity.y);
        }
        if (lockMovementUp)
        {
            isLockMovement -= 1;
        }

        if (moveRight == 1 && !facingRight)
        {
            facingRight = true;
            Flip();
        }
        else if (moveLeft == 1 && facingRight)
        {
            facingRight = false;
            Flip();
        }

        if (InputManager.GetButtonDown("Player" + player + "Attack"))
        {
            Attack();
        }
        else if (InputManager.GetButtonUp("Player" + player + "Attack"))
        {
            DoneAttack();
        }
        if (InputManager.GetButtonDown("Player" + player + "SpecialAttack"))
        {
            SpecialAttack();
        }
        else if (InputManager.GetButtonUp("Player" + player + "SpecialAttack"))
        {
            DoneSpecialAttack();
        }
    }

    private void HandleHP()
    {
        if (HP <= 0)
        {
            //SceneController.LoadEndScene(false);
        }
    }

    // restore the energy
    public void AddEnergy(float energy)
    {
        if (energy + MP > MaxEnergy)
        {
            MP = MaxEnergy;
        }
        else
        {
            MP += energy;
        }
    }

    // restore hp
    public void AddHP(float HP)
    {
        if (HP + this.HP > MaxHP)
        {
            this.HP = MaxHP;
        }
        else
        {
            this.HP += HP;
        }
    }

    // TODO rework
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        BaseItem item = collision.gameObject.GetComponent<BaseItem>();
        if (item != null)
        {
            myRb2D.velocity = currentVelocity;
        }
    }

    protected virtual void handleMovementAnimation()
    {
        if (Mathf.Abs(myRb2D.velocity.x) > 0 && myRb2D.velocity.y == 0)
        {
            // walking
            Animate.SetBool("isWalking", true);
        }
        else
        {
            // idle
            Animate.SetBool("isWalking", false);
        }

        if (myRb2D.velocity.y == 0)
        {
            // reset
            Animate.SetBool("isFalling", false);
            Animate.SetBool("isJumping", false);
        }
        if (myRb2D.velocity.y > 0)
        {
            // jumping
            Animate.SetBool("isJumping", true);
        }
        else if (myRb2D.velocity.y < 0)
        {
            // falling
            Animate.SetBool("isJumping", false);
            Animate.SetBool("isFalling", true);
        }

    }

    public void SetElectricAttack(bool isElectric)
    {
        electricAttack = isElectric;
        if (isElectric)
        {
            GetComponent<Renderer>().material = electricEffect;
        }
        else
        {
            GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
