using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
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
        if (IsDead) return;
        health -= damage;
        onHurt?.Invoke();
        if (health <= 0)
        {
            onDeath?.Invoke();
        }
    }
}
