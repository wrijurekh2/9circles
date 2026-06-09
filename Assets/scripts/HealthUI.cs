using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public float currentHealth = 10f;
    public float maxHealth = 100f;
    private Image fillImage;
    void Start()
    {

        fillImage = GetComponentsInChildren<Image>()[1];
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}
