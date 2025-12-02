using UnityEngine;

[RequireComponent(typeof(PooledObjectHelper))]
public class EnemyHealthControler : MonoBehaviour
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
        GetComponent<PooledObjectHelper>().onReset += Reset;
        Reset();
    }

    void Reset()
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
