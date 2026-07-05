using UnityEngine;

public class Stalagmites : MonoBehaviour
{
    private HealthUI healthUI;
    public float damage = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthUI = FindFirstObjectByType<HealthUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        healthUI.TakeDamage(damage);
        other.gameObject.GetComponent<PlayerMovement>().ReturnToSafePosition();
    }
}
