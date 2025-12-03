using UnityEngine;

public class HabilidadFuego : AbilityBase
{
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float tornadoLifetime = 4f;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Activate()
    {
        Vector3 clickPoint = AbilityManager.ClickPoint;

        Vector3 dir = clickPoint - player.position;
        dir.y = 0f;

        float dist = dir.magnitude;

        if (dist > maxRange)
            dir = dir.normalized * maxRange;

        Vector3 spawnPos = player.position + dir;

        GameObject tornado = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);

        Destroy(tornado, tornadoLifetime);
    }
}
