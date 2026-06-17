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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
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

}
