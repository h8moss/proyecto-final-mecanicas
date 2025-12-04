using UnityEngine;

public class VerdaderoRayo : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject rayoPrefab;
    [SerializeField] private GameObject previewPrefab;

    [Header("Stats")]
    [SerializeField] private float maxRange = 6f;

    private GameObject previewInstance;
    private Transform player;

    private void Awake()
    {
        // Tomamos al jugador automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        abilityName = "Rayo";
    }

    public override void ShowPreview()
    {
        if (previewInstance == null && previewPrefab != null)
            previewInstance = Instantiate(previewPrefab);

        if (player == null) return;

        Vector3 hoverPoint = AbilityManager.HoverPoint;
        Vector3 dir = hoverPoint - player.position;
        dir.y = 0;

        if (dir.magnitude > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 previewPos = player.position + dir;

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

        Vector3 clickPoint = AbilityManager.ClickPoint;
        Vector3 dir = clickPoint - player.position;
        dir.y = 0;

        if (dir.magnitude > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 spawnPos = player.position + dir;

        Instantiate(rayoPrefab, spawnPos, Quaternion.identity);
    }
}
