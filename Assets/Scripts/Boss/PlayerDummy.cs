using UnityEngine;

public class PlayerDummy : MonoBehaviour, IDamageable
{
    public void TakeDamage(int d) { Debug.Log($"Jugador recibió {d} daño!"); }
}