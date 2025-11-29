using JetBrains.Annotations;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 150;

    public void Curar(int cura)
    {
        health += cura;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void Dano(int dano)
    {
        health -= dano;
        if (health <= 0)
        {
            //aca va funcion para que muera
        }
    }
}
