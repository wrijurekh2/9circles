using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 1000f;
    private float currentHealth;
    private Rigidbody2D rb;

    void Start()
    {      
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        //animator.SetTrigger("Hurt");
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        rb.linearVelocity = new Vector2(0, 0);
        //animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
