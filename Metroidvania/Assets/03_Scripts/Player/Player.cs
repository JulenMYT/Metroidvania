using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool isControlLocked;

    public PlayerState currentState;

    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerCrouchState crouchState;
    public PlayerDamagedState damagedState;
    public PlayerDeathState deathState;
    public PlayerSlideState slideState;
    public PlayerAttackState attackState;
    public PlayerSpellcastState spellcastState;
    public PlayerWallJumpState wallJumpState;
    public PlayerWallSlideState wallSlideState;
    public PlayerFallState fallState;

    [Header("Core Components")]
    public Damage damage;
    public Combat combat;
    public Magic magic;
    public Health health;

    [Header("Components")]
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public Animator anim;
    public CapsuleCollider2D playerCollider;

    [Header("Movement Variables")]
    public float walkSpeed;
    public float runSpeed = 8;
    public float jumpForce;
    public float jumpCutMultiplier;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;


    public int facingDirection = 1;

    //Inputs
    public Vector2 moveInput;
    public bool runPressed;
    public bool jumpPressed;
    public bool jumpReleased;
    public bool attackPressed;
    public bool spellcastPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("WallCheck")]
    public Transform wallCheck;
    public float wallCheckRadius = 0.15f;
    public LayerMask wallLayer;
    public bool isTouchingWall;

    [Header("Crouch Check")]
    public Transform headCheck;
    public float headCheckRadius = 0.2f;

    [Header("Slide Settings")]
    public float slideDuration = 0.6f;
    public float slideSpeed = 8;
    public float slideStopDuration = 0.15f;

    public float slideHeight;
    public Vector2 slideOffset;
    public float normalHeight;
    public Vector2 normalOffset;

    private bool isSliding;

    private void Awake()
    {
        ServiceLocator.Register<Player>(this);

        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        moveState = new PlayerMoveState(this);
        crouchState = new PlayerCrouchState(this);
        damagedState = new PlayerDamagedState(this);
        deathState = new PlayerDeathState(this);
        slideState = new PlayerSlideState(this);
        attackState = new PlayerAttackState(this);
        spellcastState = new PlayerSpellcastState(this);
        wallJumpState = new PlayerWallJumpState(this);
        wallSlideState = new PlayerWallSlideState(this);
        fallState = new PlayerFallState(this);
    }

    private void Start()
    {
        rb.gravityScale = normalGravity;

        ChangeState(idleState);
    }

    private void Update()
    {
        currentState.Update();

        TryStandUp();

        if (!isSliding)
            Flip();

        HandleAnimations();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();

        CheckGrounded();
        CheckForWalls();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void TryStandUp()
    {
        if (isSliding)
        {
            anim.SetBool("isCrouching", false);
            return;
        }
    }

    public void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, normalHeight);
        playerCollider.offset = normalOffset;
    }

    public void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, slideHeight);
        playerCollider.offset = slideOffset;
    }

    public void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.linearVelocity.y > 0.1f)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void CheckForWalls()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(headCheck.position, headCheckRadius, groundLayer);
    }

    void HandleAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
    }

    void Flip()
    {
        if (currentState == deathState)
        { 
            return; 
        }

        if (moveInput.x > 0.1f)
        {
            facingDirection = 1;
        }
        else if (moveInput.x < -0.1f)
        {
            facingDirection = -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    public void AnimationFinished()
    {
        currentState.AnimationFinished();
    }

    public void OnMove(InputValue value)
    {
        if (isControlLocked)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        runPressed = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        if (isControlLocked)
            return;

        attackPressed = value.isPressed;
    }

    public void OnLeftShoulder(InputValue value)
    {
        if (value.isPressed)
        {
            magic.PreviousSpell();
        }
    }

    public void OnRightShoulder(InputValue value)
    {
        if (value.isPressed)
        {
            magic.NextSpell();
        }
    }

    public void OnSpellcast(InputValue value)
    {
        spellcastPressed = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (isControlLocked)
            return;

        if (value.isPressed)
        {
            if (isGrounded && !CheckForCeiling() || !isGrounded && isTouchingWall)
                jumpPressed = true;
            jumpReleased = false;
        }
        else //buton is released
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(headCheck.position, headCheckRadius);
    }
}
