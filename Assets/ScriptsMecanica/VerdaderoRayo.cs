using UnityEngine;

public class VerdaderoRayo : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject rayoPrefab;
    [SerializeField] private GameObject previewPrefab;

    [Header("Stats")]
    [SerializeField] private float maxRange = 6f;

    public Transform player;

    private GameObject previewInstance;

    public override void ShowPreview()
    {
        if (previewInstance == null && previewPrefab != null)
        {
            previewInstance = Instantiate(previewPrefab);
        }

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
        {
            Destroy(previewInstance);
        }
    }

    public override void Activate()
    {
        Vector3 clickPoint = AbilityManager.ClickPoint;
        Vector3 dir = clickPoint - player.position;
        dir.y = 0;

        if (dir.magnitude > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 spawnPos = player.position + dir;

        Instantiate(rayoPrefab, spawnPos, Quaternion.identity);
    }
}
