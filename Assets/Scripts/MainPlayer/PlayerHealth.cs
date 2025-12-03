using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int escudo;
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; }

    public bool IsDead { get => health <= 0; }

    public delegate void OnHealthEvent();
    public OnHealthEvent onDeath;
    public OnHealthEvent onHurt;
    public OnHealthEvent onHealed;
    public OnHealthEvent onHealthChanged;


    
    void Start()
    {
        health = maxHealth;

        onHurt += NotifyHealthChanged;
        onHealed += NotifyHealthChanged;
    }

    void NotifyHealthChanged()
    {
        onHealthChanged?.Invoke();
    }

    public void DealDamage(int damage)
    {
        int damRes;
        if (IsDead) return;
        if (escudo > 0)
        {
            damRes = escudo - damage;
            escudo -= damage;
            if (damRes > 0)
                return;
            else
            {
                health -= damRes;
                onHurt?.Invoke();
                if (health <= 0)
                {
                    onDeath?.Invoke();
                }
                return;
            }
        }
        health -= damage;
        onHurt?.Invoke();
        if (health <= 0)
        {
            onDeath?.Invoke();
        }
    }

    public void Heal(int heal)
    {
        if (IsDead) return;
        health += heal;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        onHealed?.Invoke();
    }
}
