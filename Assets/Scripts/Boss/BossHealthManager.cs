using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour, IDamageable
{
    [Header("Configuración de Salud")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Referencias UI")]
    public Slider healthBar;

    [Header("Fases Visuales")]
    public Renderer bossMesh;
    public Color phase2Color = Color.red;

    [Header("Feedback Daño (Damage Flash)")]
    public Color flashColor = new Color(1f, 0.5f, 0.5f); // Un rojo/blanco brillante
    public float flashDuration = 0.1f; // Muy rápido (0.1s)

    // Referencias internas
    private BossController controller;
    private bool isPhase2Started = false;

    // Variables para controlar el color
    private Color currentBaseColor; // Memoriza de qué color debería estar el boss (Blanco o Rojo)
    private Coroutine flashRoutine; // Para controlar que no se trabe el parpadeo

    private void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<BossController>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            healthBar.gameObject.SetActive(false); // Ocultar al inicio
        }

        if (bossMesh == null) bossMesh = GetComponentInChildren<Renderer>();

        // Guardamos el color original (probablemente blanco) como base
        if (bossMesh != null) currentBaseColor = bossMesh.material.color;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        // ACTIVAR BARRA DE VIDA AL PRIMER GOLPE
        if (healthBar != null && !healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(true);

        // --- EFECTO DE FLASH ---
        if (bossMesh != null)
        {
            // Si ya estaba parpadeando, lo paramos para reiniciar el flash
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(DamageFlash());
        }
        // ----------------------

        currentHealth -= damage;
        if (healthBar != null) healthBar.value = currentHealth;

        // Debug.Log($"BOSS HP: {currentHealth}/{maxHealth}");

        // CHEQUEO FASE 2
        if (!isPhase2Started && currentHealth <= (maxHealth * 0.5f))
        {
            isPhase2Started = true;
            TriggerPhase2();
        }

        if (currentHealth <= 0) Die();
        else
        {
            if (controller != null) controller.NotifyDamageReceived();
        }
    }

    // --- CORRUTINA PARA EL PARPADEO ---
    System.Collections.IEnumerator DamageFlash()
    {
        // 1. FORZAR COLOR DE FLASH (Intentamos ambas propiedades por si usas URP)
        if (bossMesh != null)
        {
            bossMesh.material.color = flashColor; // Standard
            if (bossMesh.material.HasProperty("_BaseColor"))
                bossMesh.material.SetColor("_BaseColor", flashColor); // URP

            // Opcional: Activar Emisión para que brille en la oscuridad
            bossMesh.material.EnableKeyword("_EMISSION");
            bossMesh.material.SetColor("_EmissionColor", flashColor);
        }

        // 2. Esperar
        yield return new WaitForSeconds(flashDuration);

        // 3. REGRESAR AL COLOR BASE
        if (bossMesh != null)
        {
            bossMesh.material.color = currentBaseColor; // Standard
            if (bossMesh.material.HasProperty("_BaseColor"))
                bossMesh.material.SetColor("_BaseColor", currentBaseColor); // URP

            // Apagar emisión
            bossMesh.material.SetColor("_EmissionColor", Color.black);
        }
    }

    void TriggerPhase2()
    {
        Debug.Log("!!! FASE 2 !!!");

        // Actualizamos el color base a ROJO
        currentBaseColor = phase2Color;

        // Aplicamos el color inmediatamente
        if (bossMesh != null) bossMesh.material.color = currentBaseColor;

        if (controller != null) controller.StartPhase2();
    }

    void Die()
    {
        Debug.Log("BOSS MUERTO");
        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (controller != null) controller.DieSequence();
        Destroy(gameObject, 4f);
    }

    // --- DEBUG ---
    [ContextMenu("DEBUG: Daño Test")]
    public void DebugDamage() { TakeDamage(10); }
}