using UnityEngine;

public class HabilidadAire : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject airePrefab;
    [SerializeField] private GameObject previewPrefab;

    [Header("Stats")]
    public Transform player;

    private GameObject previewInstance;

    public override void ShowPreview()
    {
        if (previewInstance == null && previewPrefab != null)
            previewInstance = Instantiate(previewPrefab);

        if (previewInstance != null)
        {
            Vector3 previewPos = player.position;
            previewPos.y = player.position.y;
            previewInstance.transform.position = previewPos;
        }
    }

    public override void HidePreview()
    {
        if (previewInstance != null)
            Destroy(previewInstance);
    }

    public override void Activate()
    {
        Vector3 spawnPos = player.position;

        Instantiate(airePrefab, spawnPos, Quaternion.identity);
    }
}
