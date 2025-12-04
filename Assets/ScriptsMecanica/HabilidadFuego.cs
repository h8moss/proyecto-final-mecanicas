using UnityEngine;

public class HabilidadFuego : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private GameObject tornadoPreviewPrefab;

    [Header("Stats")]
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float tornadoLifetime = 4f;

    private GameObject previewInstance;
    private Transform player;

    private void Awake()
    {
        // Buscar al jugador automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        abilityName = "Fuego";
    }

    public override void ShowPreview()
    {
        if (previewInstance == null && tornadoPreviewPrefab != null)
            previewInstance = Instantiate(tornadoPreviewPrefab);

        if (player == null) return;

        // Usar el punto del mouse (HoverPoint)
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

        Quaternion spawnRotation = Quaternion.Euler(-90f, 0f, 0f);
        GameObject tornado = Instantiate(tornadoPrefab, spawnPos, spawnRotation);

        Destroy(tornado, tornadoLifetime);
    }
}
