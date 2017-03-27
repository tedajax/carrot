using UnityEngine;

public class HealthProperty : MonoBehaviour
{
    public float maxHealth = 1f;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get { return CurrentHealth == 0f; } }

    public delegate void OnDeathHandler(GameObject sender);
    private event OnDeathHandler onDeath;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Subscribe(OnDeathHandler onDeath)
    {
        this.onDeath += onDeath;
    }

    public void Unsubscribe(OnDeathHandler onDeath)
    {
        this.onDeath -= onDeath;
    }

    public void AdjustHealth(float amount)
    {
        if (IsDead) {
            return;
        }

        CurrentHealth += amount;

        if (CurrentHealth > maxHealth) {
            CurrentHealth = maxHealth;
        }
        else if (CurrentHealth <= 0f) {
            CurrentHealth = 0f;
            if (onDeath != null) {
                onDeath(gameObject);
            }
        }
    }

    public void Damage(float amount)
    {
        AdjustHealth(Mathf.Clamp(-amount, -amount, 0f));
    }

    public void Heal(float amount)
    {
        AdjustHealth(Mathf.Clamp(amount, 0, amount));
    }
}