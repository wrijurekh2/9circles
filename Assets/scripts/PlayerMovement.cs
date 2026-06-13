using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private InputSystem_Actions playerInput;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool Grounded;
    public int currDashCharges = 3;
    public int maxDashCharges = 3; 
    public float dashCooldown = 5f;
    public float timer = 0f;
    public bool isDashing = false;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public float dashForce = 10f;
    public Transform wallCheck;
    private float wallCheckRadius = 0.1f;
    private bool isTouchingWall;
    private float lastDirection = 1f;

    void Start()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    IEnumerator Dash()
    { 
        rb.linearVelocity = new Vector2(dashForce * lastDirection, rb.linearVelocity.y);
        isDashing = true;
        animator.SetBool("IsDashing", isDashing);
        currDashCharges -= 1;

        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        isDashing = false;
        animator.SetBool("IsDashing", isDashing);
    }

    void Update()
    {
        movement.x = playerInput.Player.Move.ReadValue<Vector2>().x;
        if (movement.x != 0) lastDirection = movement.x;

        // Check if grounded
        Grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isTouchingWall = Physics2D.OverlapCircle(
            wallCheck.position,
            wallCheckRadius,
            groundLayer
        );

        

        // keep track of timer
        if (currDashCharges < maxDashCharges)
        {
            timer += Time.deltaTime;
            if (timer >= dashCooldown)
            {
                currDashCharges += 1;
                timer = 0f;
            }
        }

        // Jump
        if (playerInput.Player.Jump.WasPressedThisFrame() && Grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

        if (playerInput.Player.Dash.WasPressedThisFrame() && !isDashing 
                && currDashCharges > 0)
        {   
            StartCoroutine(Dash());
            animator.SetTrigger("Dash");
        }

        /*if(playerInput.Player.Attack.WasPressedThisFrame())
        {
            animator.SetTrigger("Attack1");
        }*/

        

        // Animations

        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            animator.SetBool("IsRunning", true);
            if(!isDashing)
                GetComponent<SpriteRenderer>().flipX = movement.x > 0;
        }
        else
        {
            animator.SetBool("IsRunning", false);
            if(!isDashing)
                GetComponent<SpriteRenderer>().flipX = lastDirection > 0;
        }
        
        
        animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f);
        animator.SetBool("IsGrounded", Grounded);
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
           rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}