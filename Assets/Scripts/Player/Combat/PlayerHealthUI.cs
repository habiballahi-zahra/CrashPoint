using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Image healthFill;

    void Start()
    {
        UpdateHealth();
        playerHealth.onHit += UpdateHealth;
        playerHealth.onDeath += UpdateHealth;
    }

    void UpdateHealth()
    {
        float percent = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        healthFill.fillAmount = percent;
        healthFill.color = Color.Lerp(Color.red,Color.gray, percent);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.onHit -= UpdateHealth;
            playerHealth.onDeath -= UpdateHealth;
        }
    }
}
