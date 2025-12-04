using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour, IDamageable
{
    [Header("Configuraci�n de Salud")]
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Referencias UI")]
    public Slider healthBar;
    [Header("MUSICA DE BATALLA")]
    public AudioSource musicSource; // Arrastra el Audio Source aquí
    public AudioClip phase1Loop;    // Canción normal
    public AudioClip phase2Loop;
    private float defaultVolume;

    [Header("Fases Visuales")]
    public Renderer bossMesh;
    public Color phase2Color = Color.red;

    [Header("Feedback Da�o (Damage Flash)")]
    public Color flashColor = new Color(1f, 0.5f, 0.5f); // Un rojo/blanco brillante
    public float flashDuration = 0.1f; // Muy r�pido (0.1s)

    // Referencias internas
    private BossController controller;
    private bool isPhase2Started = false;
    [Tooltip("Tiempo en segundos que tarda en cambiar la canción")]
    public float musicFadeTime = 2.0f;
    // Variables para controlar el color
    private Color currentBaseColor; // Memoriza de qu� color deber�a estar el boss (Blanco o Rojo)
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

        if (musicSource != null && phase1Loop != null)
        {
            defaultVolume = musicSource.volume; // Guardamos el volumen que pusiste en el Inspector
            musicSource.clip = phase1Loop;
            musicSource.loop = true;
            musicSource.Play();
        }
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

            // Opcional: Activar Emisi�n para que brille en la oscuridad
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

            // Apagar emisi�n
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
        if (musicSource != null && phase2Loop != null)
        {
            StartCoroutine(FadeToNextSong(phase2Loop));
        }

       

        if (controller != null) controller.StartPhase2();
    }
    IEnumerator FadeToNextSong(AudioClip nextClip)
    {
        // 1. BAJAR VOLUMEN (Fade Out)
        float startVol = musicSource.volume;
        float speed = 1f / (musicFadeTime / 2f); // Dividimos el tiempo entre bajar y subir

        for (float t = 0; t < 1; t += Time.deltaTime * speed)
        {
            musicSource.volume = Mathf.Lerp(startVol, 0, t);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Stop();

        // 2. CAMBIAR DISCO
        musicSource.clip = nextClip;
        musicSource.Play();

        // 3. SUBIR VOLUMEN (Fade In)
        for (float t = 0; t < 1; t += Time.deltaTime * speed)
        {
            musicSource.volume = Mathf.Lerp(0, defaultVolume, t);
            yield return null;
        }
        musicSource.volume = defaultVolume;
    }

    // --- FADE OUT AL MORIR ---
    IEnumerator FadeOutMusic()
    {
        float startVol = musicSource.volume;
        for (float t = 0; t < 1; t += Time.deltaTime * 0.5f) // 2 segundos para apagarse
        {
            musicSource.volume = Mathf.Lerp(startVol, 0, t);
            yield return null;
        }
        musicSource.Stop();
    }
    void Die()
    {
        Debug.Log("BOSS MUERTO");
        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (controller != null) controller.DieSequence();
        // Destroy(gameObject, 4f);
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(4);
    }

    // --- DEBUG ---
    [ContextMenu("DEBUG: Da�o Test")]
    public void DebugDamage() { TakeDamage(10); }
}