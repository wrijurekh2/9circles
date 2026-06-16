using System.Collections;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem;

enum EnemyMode
    {
        Idle,
        Combat,
        Death
    }
public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyMode currentMode;
    private Transform player;
    public float minPatrol = -240.34f;
    public float maxPatrol = -217.18f;
    public float detectionRange = 10f;
    private Vector2 movement;
    public float moveSpeed = 5f;
    private float attackRange = 5f;
    public float maxHealth = 100f;
    private float currentHealth;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public LayerMask playerLayer;
    public Transform attackPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player =  GameObject.FindWithTag("Player").transform;
        currentMode = EnemyMode.Idle;
        movement.x = 1;
        currentHealth = maxHealth;
        animator.SetBool("Grounded", true);
        
    }

    void attack()
    {
        animator.SetTrigger("Attack1");

        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, 
            attackRange, playerLayer);

        if (hitPlayer != null)
        {
            player.GetComponent<HealthUI>().TakeDamage(20);
        }
        
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        rb.linearVelocity = new Vector2(0, 0);
        animator.SetTrigger("Death");
        animator.SetBool("noBlood", false);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.x < 0)
            transform.localScale = new Vector3(-2, 2, 1);
        else
            transform.localScale = new Vector3(2, 2, 1);
        if (currentMode == EnemyMode.Idle)
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
            animator.SetInteger("AnimState", 1);
            if (movement.x > 0 && transform.position.x >= maxPatrol)
            {
                movement.x = -1;
            }
            else if (movement.x < 0 && transform.position.x <= minPatrol)
            {
                movement.x = 1;
            }

            
        }

        if (currentMode == EnemyMode.Combat)
        { 
            movement.x = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
            if (Time.time >= nextAttackTime){
                if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }

        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            currentMode = EnemyMode.Combat;
        }

        if (movement.x != 0)
        {
            
        }

        if (currentMode == EnemyMode.Death)
        {
            
        }


        

    }
}
