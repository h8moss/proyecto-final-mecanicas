using UnityEngine;

public class HabilidadTierra : AbilityBase
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bolaPrefab;

    [Header("Stats")]
    [SerializeField] private float maxRange = 8f;
    [SerializeField] private float velocidad = 10f;

    private Transform player;
    private LineRenderer previewLine;

    private void Awake()
    {
        // Tomamos al jugador automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Crear LineRenderer para preview
        GameObject lrObj = new GameObject("PreviewLine");
        previewLine = lrObj.AddComponent<LineRenderer>();
        previewLine.startWidth = 0.1f;
        previewLine.endWidth = 0.1f;
        previewLine.positionCount = 2;
        previewLine.material = new Material(Shader.Find("Sprites/Default"));
        previewLine.startColor = Color.yellow;
        previewLine.endColor = Color.yellow;
        previewLine.enabled = false;

        abilityName = "Tierra";
    }

    public override void ShowPreview()
    {
        if (player == null) return;

        Vector3 hoverPoint = AbilityManager.HoverPoint;
        Vector3 dir = hoverPoint - player.position;
        dir.y = 0;

        if (dir.magnitude > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 endPoint = player.position + dir;

        previewLine.enabled = true;
        previewLine.SetPosition(0, player.position + Vector3.up * 1f);
        previewLine.SetPosition(1, endPoint + Vector3.up * 0.1f);
    }

    public override void HidePreview()
    {
        if (previewLine != null)
            previewLine.enabled = false;
    }

    public override void Activate()
    {
        if (player == null) return;

        Vector3 hoverPoint = AbilityManager.HoverPoint;
        Vector3 dir = hoverPoint - player.position;
        dir.y = 0;

        if (dir.magnitude > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 spawnPos = player.position;

        GameObject bola = Instantiate(bolaPrefab, spawnPos, Quaternion.identity);

        if (dir != Vector3.zero)
            bola.transform.rotation = Quaternion.LookRotation(dir);

        Rigidbody rb = bola.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir.normalized * velocidad;

        HidePreview();
    }
}
