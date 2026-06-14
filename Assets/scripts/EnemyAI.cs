using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player =  GameObject.FindWithTag("Player").transform;
        currentMode = EnemyMode.Idle;
        movement.x = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        if (currentMode == EnemyMode.Idle)
        {
            rb.linearVelocity = new Vector2(movement.x * moveSpeed, 0);
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


        

    }
}
