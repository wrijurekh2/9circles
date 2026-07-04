using System.Collections;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    public float detectionRange = 10f;
    private Vector2 movement;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public bool aggro;
    public float lungeSpeed = 5f;
    public float minX = -13f;
    public float maxX = 14f;
    private BoxCollider2D box;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player =  GameObject.FindWithTag("Player").transform;
        box = GetComponentsInChildren<BoxCollider2D>()[1];
    }

    void Update()
    {
        aggro = Vector2.Distance(player.position, transform.position) < detectionRange;

        if (aggro)
            animator.SetBool("Aggro", true);
        else if (!aggro)
            animator.SetBool("Aggro", false);
    }

    public void LungeForward()
    {
        rb.linearVelocity = new Vector2(-lungeSpeed * transform.localScale.x, rb.linearVelocity.y);
    }

    public void LungeBack()
    {
        rb.linearVelocity = new Vector2(lungeSpeed * transform.localScale.x, rb.linearVelocity.y);
    }

    public void StopLunge()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public void EnableBox()
    {
        box.enabled = true;
    }

    public void DisableBox()
    {
        box.enabled = false;
    }
}
