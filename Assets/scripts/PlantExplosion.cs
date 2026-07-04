using UnityEngine;

public class PlantExplosion : MonoBehaviour
{
    private Vector2 plantPosition;
    private GameObject player;
    private Vector2 playerPosition;
    public float detectionRadius;
    private Animator animator;
    void Start()
    {
        plantPosition = transform.position;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        if (Vector2.Distance(plantPosition, playerPosition) <= detectionRadius)
        {
            animator.SetTrigger("Explode");
        }
    }

    
}
