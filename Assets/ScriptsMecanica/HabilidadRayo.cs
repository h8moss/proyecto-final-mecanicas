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
    public PlayerHealth playerHealth;
    private Transform player;

    public bool IsActive => shieldInstance != null && shieldInstance.activeSelf;

    private void Start()
    {
        player = playerHealth.transform;
    }

    public override void ShowPreview()
    {
        if (previewInstance == null)
        {
            previewInstance = Instantiate(previewPrefab);
        }

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
        if (shieldInstance == null)
            shieldInstance = Instantiate(shieldPrefab);

        currentShieldHP = maxShieldHP;
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
        {
            Deactivate();
        }
    }
}
