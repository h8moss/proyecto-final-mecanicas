using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int escudo;
    [SerializeField] private int maxHealth;
    private int health;

    public int MaxHealth { get => maxHealth; }
    public int Health { get => health; }

    public bool IsDead { get => health <= 0; }

    public delegate void OnDeath();
    public OnDeath onDeath;
    public delegate void OnHurt();
    public OnDeath onHurt;

    
    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        int damRes;
        if (IsDead) return;
        if (escudo > 0)
        {
            damRes = escudo - damage;
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
    }
}
