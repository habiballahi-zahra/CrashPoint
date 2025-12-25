using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public delegate void OnDeathEvent();
    public event OnDeathEvent onDeath;

    public delegate void OnHitEvent();
    public event OnHitEvent onHit;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        onHit?.Invoke(); // 🔥 این خط خیلی مهمه

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        onDeath?.Invoke();
    }
}
