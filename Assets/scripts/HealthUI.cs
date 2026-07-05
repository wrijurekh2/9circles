using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class HealthUI : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100f;
    private Image fillImage;
    private Image bufferImage;
    private Image bufferImage2;
    private Animator heartAnimator;
    private bool canTakeDamage = true;
    private float Itimer;

    
    void Start()
    {
        currentHealth = maxHealth;
        fillImage = GetComponentsInChildren<Image>()[3];
        bufferImage = GetComponentsInChildren<Image>()[1];
        bufferImage2 = GetComponentsInChildren<Image>()[2]; 
        heartAnimator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;
        currentHealth -= damage;
        StartCoroutine(Iframes(1f));
    }

    IEnumerator DamageOverTime(float damagePerTick, float tickInterval, float duration)
    {
        float elapsed = 0;

        while (elapsed <= duration)
        {
            currentHealth -= damagePerTick;
            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }

    public void TakeDotDamage(float damagePerTick, float tickInterval, float duration)
    {
        StartCoroutine(DamageOverTime(damagePerTick, tickInterval, duration));
    }

    IEnumerator Iframes(float IframeDuration)
    {
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime(IframeDuration);
        canTakeDamage = true;
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float targetFill = currentHealth / maxHealth;
        if (targetFill < fillImage.fillAmount)
        {  
            bufferImage.fillAmount = fillImage.fillAmount;         
            fillImage.fillAmount = targetFill;
            bufferImage2.fillAmount = targetFill;
        }
        else if(targetFill > fillImage.fillAmount)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, 2f * Time.deltaTime); 
            bufferImage2.fillAmount = targetFill;   
            bufferImage.fillAmount = fillImage.fillAmount;      
        }
        if (bufferImage.fillAmount > fillImage.fillAmount)
        {
            bufferImage.fillAmount = Mathf.Lerp(
                bufferImage.fillAmount, fillImage.fillAmount, 2f * Time.deltaTime);
        }
        heartAnimator.speed = 0.3f + 2.5f * (1f - (currentHealth / maxHealth));
    }
}
