using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool Grounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        // Check if grounded
        Grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Jump
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            Debug.Log("Jumping");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if(Input.GetButtonDown("Fire1"))
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
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }
}