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
    public int damage;
    private HealthUI healthUI;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        box = attackPoint.GetComponentInChildren<BoxCollider2D>();
        box.enabled = false;
        healthUI = FindAnyObjectByType<HealthUI>();
    }

    void Update()
    {
        if (healthUI.currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canRecieveInput)
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
        if (!canRecieveInput)
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
        this.damage = damage;
    }

    public void Die()
    {
        Destroy(gameObject);
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

    public void SetHitboxA1F1()
    {
        attackPoint.transform.localPosition = new Vector2(-1.43f, 0.47f);
        box.offset = new Vector2(-0.1629119f, 0.4499743f);
        box.size = new Vector2(0.9422464f, 1.823358f);
    }

    public void SetHitboxA1F2()
    {
        attackPoint.transform.localPosition = new Vector2(-1.7f, -0.49f);
        box.offset = new Vector2(-0.07674599f, 0.7276181f);
        box.size = new Vector2(1.382647f, 2.378646f);
    }

    public void SetHitboxA2F1()
    {
        attackPoint.transform.localPosition = new Vector2(-2.89f, -0.61f);
        box.offset = new Vector2(0.1805029f, 0.904477f);
        box.size = new Vector2(2.572424f, 2.989613f);
    }

    public void SetHitboxA3F1()
    {
        attackPoint.transform.localPosition = new Vector2(-2.89f, -0.61f);
        box.offset = new Vector2(1.080875f, 0.3739003f);
        box.size = new Vector2(0.7716789f, 0.706526f);
    }

    public void SetHitboxA3F2()
    {
        attackPoint.transform.localPosition = new Vector2(-2.97f, -0.61f);
        box.offset = new Vector2(0.9281335f, 0.4542907f);
        box.size = new Vector2(1.463037f, 0.9316189f);
    }
}
