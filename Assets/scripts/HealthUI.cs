using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public float currentHealth = 10f;
    public float maxHealth = 100f;
    private Image fillImage;
    private Image bufferImage;
    private Animator heartAnimator;
    void Start()
    {
        currentHealth = maxHealth;
        fillImage = GetComponentsInChildren<Image>()[2];
        bufferImage = GetComponentsInChildren<Image>()[1];
        heartAnimator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float targetFill = currentHealth / maxHealth;
        if (targetFill < fillImage.fillAmount)
        {           
            bufferImage.fillAmount = Mathf.Lerp(bufferImage.fillAmount, targetFill, 2f * Time.deltaTime); 
            fillImage.fillAmount = targetFill;
        }
        else if(targetFill > fillImage.fillAmount)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, 2f * Time.deltaTime); 
            bufferImage.fillAmount = targetFill;         
        }
        heartAnimator.speed = 0.3f + 2.5f * (1f - (currentHealth / maxHealth));
    }
}
