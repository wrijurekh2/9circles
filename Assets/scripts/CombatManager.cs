using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    public bool canRecieveInput;
    public bool inputRecieved;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public BoxCollider2D box;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        box = attackPoint.GetComponentInChildren<BoxCollider2D>();
        
    }

    void Update()
    {
        box.enabled = false;
    }

    void Attack1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canRecieveInput)
            {
                inputRecieved = true;
                canRecieveInput = false;
            }
        }
        else
        {
            return;
        }
    }

    public void InputManager()
    {
        if(!canRecieveInput)
        {
            canRecieveInput = true;
        }
        else
        {
            canRecieveInput = false; 
        }
    }


    public void Attack(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,
            attackRange, enemyLayers);
        
        //collider2D hitEnemies2 ;

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(damage);
        }

        /*if (hitEnemies.Length > 0)
        {
            StartCoroutine(ImpactFreeze(0.08f));
        }*/
    }

    IEnumerator ImpactFreeze(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        
    }

    public void EnableHitbox()
    {
        box.enabled = true;
    }

    public void DisableHitbox()
    {
        box.enabled = false;
    }

    

}
