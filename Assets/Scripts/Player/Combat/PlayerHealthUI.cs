using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Image healthFill;

    void Start()
    {
        UpdateHealth();
        playerHealth.onHealthChanged += UpdateHealth;
        playerHealth.onDeath += UpdateHealth;
    }

    void UpdateHealth()
    {
        float percent = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        healthFill.fillAmount = percent;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged -= UpdateHealth;
            playerHealth.onDeath -= UpdateHealth;
        }
    }
}
