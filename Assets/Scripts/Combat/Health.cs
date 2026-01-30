using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public delegate void HealthEvent();
    public event HealthEvent onHealthChanged;

    public delegate void OnDeathEvent();
    public event OnDeathEvent onDeath;
    public event HealthEvent onHit;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(); // مقدار اولیه
        onHit?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        onHealthChanged?.Invoke();
        onHit?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke();
        onHit?.Invoke();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        onDeath?.Invoke();
    }
}
