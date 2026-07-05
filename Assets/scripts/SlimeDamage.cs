using UnityEngine;

public class SlimeDamage : MonoBehaviour
{
    private HealthUI healthUI;
    private BoxCollider2D box;

    void Start()
    {
        healthUI = FindAnyObjectByType<HealthUI>();
        box = GetComponent<BoxCollider2D>();
        box.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        healthUI.TakeDamage(40);
        Debug.Log("one singular instance of 40 damage dealt");
    }
}
