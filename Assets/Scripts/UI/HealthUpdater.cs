using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthUpdater : MonoBehaviour
{
    private Image image;
    private PlayerHealth health;

    void Start()
    {
        health = PlayerLocator.Player.GetComponent<PlayerHealth>();
        health.onHealthChanged += UpdateUI;
        image = GetComponent<Image>();
    }

    void UpdateUI()
    {
        image.fillAmount = (float)health.Health / health.MaxHealth;
    }
}
