using UnityEngine;

public class HabilidadRayo : AbilityBase
{
    [Header("Shield Settings")]
    public GameObject shieldVisual;
    public int maxShieldHP = 50;

    private int currentShieldHP;
    private PlayerHealth playerHealth;

    public bool IsActive => shieldVisual.activeSelf;

    private void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();

        playerHealth.escudo = this;

        shieldVisual.SetActive(false);
    }

    public override void Activate()
    {
        currentShieldHP = maxShieldHP;
        shieldVisual.SetActive(true);
    }

    public void Deactivate()
    {
        shieldVisual.SetActive(false);
    }

    public bool AbsorbDamage(int amount)
    {
        if (!IsActive)
            return false;

        currentShieldHP -= amount;

        if (currentShieldHP <= 0)
            Deactivate();

        return true;
    }
}
