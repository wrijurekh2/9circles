using System.Collections;
using System.Diagnostics;
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

    private bool isDashing = false;

    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public float dashForce = 15f;

    void Start()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    IEnumerator Dash()
    {
        rb.linearVelocity = new Vector2(dashForce * movement.x, rb.linearVelocity.y);
        isDashing = true;
        yield return new WaitForSeconds(0.2f);
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        isDashing = false;
        
    }

    void Update()
    {
        movement.x = playerInput.Player.Move.ReadValue<Vector2>().x;

        // Check if grounded
        Grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Jump
        if (playerInput.Player.Jump.WasPressedThisFrame() && Grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (playerInput.Player.Dash.WasPressedThisFrame() && !isDashing)
        {
            UnityEngine.Debug.Log("dashing");
            StartCoroutine(Dash());
        }

        if(playerInput.Player.Attack.WasPressedThisFrame())
        {
            animator.SetTrigger("Attack1");
        }

        // Animations

        if (movement.x != 0)
        {
            animator.SetBool("IsMoving", true);
            animator.SetInteger("AnimState", 1);
            GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetInteger("AnimState", 0);
        }

        animator.SetBool("IsJumping", rb.linearVelocity.y > 0.1f);
        animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f);
        animator.SetBool("Grounded", Grounded);
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}