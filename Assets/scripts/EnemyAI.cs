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
    public float moveSpeed = 3f;
    public float attackRange = 1f;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player =  GameObject.FindWithTag("Player").transform;
        currentMode = EnemyMode.Idle;
        movement.x = 1;
        currentHealth = maxHealth;
        
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
        GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        if (currentMode == EnemyMode.Idle)
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
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
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                animator.SetTrigger("Attack1");
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
