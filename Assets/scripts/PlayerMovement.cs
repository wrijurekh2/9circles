using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float moveSpeed = 5f;
    float acceleration = 8f;
    float deceleration = 8f;
    float velPower = 0.9f;
    float frictionAmount = 0.02f;
    public float lastDirection = 1f;
    private Vector2 moveInput;

    [Header("Jump")]
    public float jumpForce = 10f;
    float lastGroundedTime;
    float lastJumpTime;
    bool isJumping;

    [Header("Dash")]
    public float dashForce = 10f;
    public int currDashCharges = 3;
    public int maxDashCharges = 3;
    public float dashCooldown = 5f;
    public float timer = 0f;
    public bool isDashing = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public bool Grounded;

    [Header("Wall Check")]
    public Transform wallCheck;
    private float wallCheckRadius = 0.1f;
    private bool isTouchingWall;

    [Header("Grapple")]
    float postGrappleWindow = 0f;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private InputSystem_Actions playerInput;
    private GrapplingHook grapplingHook;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        grapplingHook = GetComponent<GrapplingHook>();
    }

    void Update()
    {
        moveInput.x = playerInput.Player.Move.ReadValue<Vector2>().x;
        if (moveInput.x != 0) lastDirection = moveInput.x;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -49, 49),
            transform.position.y,
            transform.position.z
        );
    

        #region Grounded and Wall Check
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer);
        #endregion

        #region Timers
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        #endregion

        #region Dash Charge Timer
        if (currDashCharges < maxDashCharges)
        {
            timer += Time.deltaTime;
            if (timer >= dashCooldown)
            {
                currDashCharges += 1;
                timer = 0f;
            }
        }
        #endregion

        #region Jump

        if(playerInput.Player.Jump.WasPressedThisFrame() && postGrappleWindow > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse);
        }

        else if (playerInput.Player.Jump.WasPressedThisFrame() && Grounded)
            Jump();

        else if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
            Jump();

        if (Grounded)
        {
            lastGroundedTime = 0.15f;
            isJumping = false;
        }
        #endregion

        #region Dash
        if (playerInput.Player.Dash.WasPressedThisFrame() && !isDashing && currDashCharges > 0)
        {
            StartCoroutine(Dash());
            animator.SetTrigger("Dash");
        }
        #endregion

        #region grapple
        if(grapplingHook.recentlyGrappled)
        {
            postGrappleWindow = 0.3f;
            grapplingHook.recentlyGrappled = false;
        }

        postGrappleWindow -= Time.deltaTime;

        #endregion

        #region Attack
        if (playerInput.Player.Attack.WasPressedThisFrame())
            CombatManager.instance.inputRecieved = true;
        #endregion

        #region Animations
        if (moveInput.x != 0)
        {
            animator.SetBool("IsRunning", true);
            if (!isDashing)
            {
                if (moveInput.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            if (!isDashing)
            {
                if (lastDirection > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }

        animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f);
        animator.SetBool("IsGrounded", Grounded);
        #endregion
    }

    void FixedUpdate()
    {
        #region Run
        if (!isDashing && !grapplingHook.isGrappling)
        {
            float targetSpeed = moveInput.x * moveSpeed;
            float speedDif = targetSpeed - rb.linearVelocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right);
        }
        #endregion

        #region Friction
        if (Grounded && Mathf.Abs(moveInput.x) < 0.01f && !grapplingHook.isGrappling)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion
    }
    #endregion

    #region Movement Methods
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        lastGroundedTime = 0;
        lastJumpTime = 0;
        animator.SetTrigger("Jump");
    }

    public void OnJump()
    {
        lastJumpTime = 0.15f;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("IsDashing", isDashing);
        currDashCharges -= 1;
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Default"),
            LayerMask.NameToLayer("Enemy"),
            true
        );

        if (!Grounded)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(dashForce * lastDirection, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(dashForce * lastDirection, rb.linearVelocity.y);
        }

        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        rb.gravityScale = 2;
        isDashing = false;
        animator.SetBool("IsDashing", isDashing);
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Default"),
            LayerMask.NameToLayer("Enemy"),
            false
        );
    }
    #endregion
}