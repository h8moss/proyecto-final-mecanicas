using UnityEngine;

public class HabilidadFuego : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private GameObject tornadoPreviewPrefab;

    [Header("Stats")]
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float tornadoLifetime = 4f;

    public Transform player;

    private GameObject previewInstance;

    public override void ShowPreview()
    {
        if (previewInstance == null)
            previewInstance = Instantiate(tornadoPreviewPrefab);

        // Usar SIEMPRE el punto del mouse (HoverPoint)
        Vector3 clickPoint = AbilityManager.HoverPoint;

        Vector3 dir = clickPoint - player.position;
        dir.y = 0;

        float dist = dir.magnitude;
        if (dist > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 previewPos = player.position + dir;

        previewInstance.transform.position = previewPos;
    }

    public override void HidePreview()
    {
        if (previewInstance != null)
            Destroy(previewInstance);
    }

    public override void Activate()
    {
        // El clickPoint YA viene actualizado del mouse-click real
        Vector3 clickPoint = AbilityManager.ClickPoint;

        Vector3 dir = clickPoint - player.position;
        dir.y = 0;

        float dist = dir.magnitude;
        if (dist > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 spawnPos = player.position + dir;

        GameObject tornado = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);

        Destroy(tornado, tornadoLifetime);
    }
}
