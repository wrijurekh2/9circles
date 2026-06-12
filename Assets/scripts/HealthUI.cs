using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public float currentHealth = 10f;
    public float maxHealth = 100f;
    private Image fillImage;
    private Animator heartAnimator;
    void Start()
    {
        currentHealth = maxHealth;
        fillImage = GetComponentsInChildren<Image>()[1];
        heartAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float targetFill = currentHealth / maxHealth;
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, 5f * Time.deltaTime);
        heartAnimator.speed = 0.3f + 2.5f * (1f - (currentHealth / maxHealth));
    }
}
