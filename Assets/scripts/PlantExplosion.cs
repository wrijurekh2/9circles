using UnityEngine;

public class PlantExplosion : MonoBehaviour
{
    private GameObject player;
    public float detectionRadius;
    private Animator animator;
    private CapsuleCollider2D explosionHitbox;
    private int idleStateHash = Animator.StringToHash("Base Layer.ExplodingPlantIdle");
    private HealthUI healthUI;
    public float damageDealtPerTick = 5f;
    public float tickInterval = 1f;
    public float tickDuration = 4f;

    private CapsuleCollider2D playerHitbox;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        explosionHitbox = GetComponentInChildren<CapsuleCollider2D>();
        healthUI = FindAnyObjectByType<HealthUI>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (Vector2.Distance(transform.position, player.transform.position) <= detectionRadius && stateInfo.fullPathHash == idleStateHash)
        {
            animator.SetTrigger("Explode");
        }
    }

    void TriggerDamage()
    {
        Vector2 point = explosionHitbox.transform.position + (Vector3)explosionHitbox.offset;
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider2D hit = Physics2D.OverlapCapsule(point, explosionHitbox.size, explosionHitbox.direction, 0f, playerMask);
        if(hit != null)
        {
            healthUI.TakeDotDamage(damageDealtPerTick, tickInterval, tickDuration);
        }
    }

    void UntriggerDamage()
    {
        explosionHitbox.enabled = false;
    }

    
    
}
