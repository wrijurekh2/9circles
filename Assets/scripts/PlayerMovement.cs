using System.Collections;
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
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;


    void Start()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
        
    }

    IEnumerator Dash()
    { 
        Debug.Log("Dash started, charges before: " + currDashCharges);
        isDashing = true;
        animator.SetBool("IsDashing", isDashing);
        currDashCharges -= 1;
        Debug.Log("Charges after decrease: " + currDashCharges);
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Default"),
            LayerMask.NameToLayer("Enemy"), 
            true
        );
        

        if(!Grounded)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(dashForce * lastDirection, 0);
        }
        else if(Grounded)
        {
            rb.linearVelocity = new Vector2(dashForce * lastDirection, rb.linearVelocity.y);
        }

        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        rb.gravityScale = 2;
        isDashing = false;
        animator.SetBool("IsDashing", isDashing);
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Default"),
            LayerMask.NameToLayer("Enemy"), 
            false
        );
    }

    void Attack()
    {
        animator.SetTrigger("Attack1");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,
         attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(20);
        }
    }
    

    void Update()
    {
        movement.x = playerInput.Player.Move.ReadValue<Vector2>().x;
        if (movement.x != 0) lastDirection = movement.x;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -49, 49),
            transform.position.y,
            transform.position.z
        );

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

        if (Time.time >= nextAttackTime)
        {
            if(playerInput.Player.Attack.WasPressedThisFrame())
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        

        // Animations

        if (movement.x != 0)
        {
            animator.SetBool("IsRunning", true);
            if (!isDashing)
            {
                if (movement.x > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
            if(!isDashing)
            {
                if (lastDirection > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
            }
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