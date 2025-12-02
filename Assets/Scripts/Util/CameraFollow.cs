using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothness;
    private Vector3 offset;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerLocator.Player;
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime*smoothness);
    }
}
