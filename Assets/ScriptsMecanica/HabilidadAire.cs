using UnityEngine;

public class HabilidadAire : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject airePrefab;
    [SerializeField] private GameObject previewPrefab;

    private Transform player;
    private GameObject previewInstance;

    private void Awake()
    {
        // Tomamos al jugador automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public override void ShowPreview()
    {
        if (previewInstance == null && previewPrefab != null)
            previewInstance = Instantiate(previewPrefab);

        if (player == null) return;

        Vector3 previewPos = player.position;
        previewPos.y = player.position.y;

        if (previewInstance != null)
            previewInstance.transform.position = previewPos;
    }

    public override void HidePreview()
    {
        if (previewInstance != null)
            Destroy(previewInstance);
    }

    public override void Activate()
    {
        if (player == null) return;

        Vector3 spawnPos = player.position;

        Instantiate(airePrefab, spawnPos, Quaternion.identity);
    }
}
