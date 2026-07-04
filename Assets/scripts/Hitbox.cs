using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        StartCoroutine(ImpactFrame());
        enemy.GetComponent<EnemyHealth>().TakeDamage(CombatManager.instance.damage);
        
    }

    IEnumerator ImpactFrame()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }
}
