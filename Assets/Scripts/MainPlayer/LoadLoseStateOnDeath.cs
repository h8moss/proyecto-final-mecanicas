using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class LoadLoseStateOnDeath : MonoBehaviour
{
    PlayerHealth health;

    void OnDeath()
    {
        SceneManager.LoadScene(3);
    }

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        health.onDeath += OnDeath;
    }
}
