using UnityEngine;

public class HabilidadRayo : AbilityBase
{
    [Header("Shield Settings")]
    public GameObject shieldPrefab;
    public int maxShieldHP = 50;

    [Header("Preview Settings")]
    public GameObject previewPrefab;

    private GameObject shieldInstance;
    private GameObject previewInstance;

    private int currentShieldHP;
    private Transform player;
    private PlayerHealth playerHealth;

    public bool IsActive => shieldInstance != null && shieldInstance.activeSelf;

    private void Awake()
    {
        // Buscamos al jugador automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    public override void ShowPreview()
    {
        if (previewInstance == null && previewPrefab != null)
            previewInstance = Instantiate(previewPrefab);

        if (player == null) return;

        previewInstance.transform.position = player.position;
        previewInstance.transform.rotation = player.rotation;
    }

    public override void HidePreview()
    {
        if (previewInstance != null)
            Destroy(previewInstance);
    }

    public override void Activate()
    {
        if (player == null) return;

        if (shieldInstance == null && shieldPrefab != null)
            shieldInstance = Instantiate(shieldPrefab);

        currentShieldHP = maxShieldHP;

        if (playerHealth != null)
            playerHealth.escudo = currentShieldHP;

        shieldInstance.transform.position = player.position;
        shieldInstance.transform.rotation = player.rotation;
        shieldInstance.transform.SetParent(player);

        shieldInstance.SetActive(true);

        HidePreview();
    }

    public void Deactivate()
    {
        if (shieldInstance != null)
            shieldInstance.SetActive(false);
    }

    private void Update()
    {
        if (playerHealth == null) return;

        currentShieldHP = playerHealth.escudo;

        if (currentShieldHP <= 0)
            Deactivate();
    }
}
