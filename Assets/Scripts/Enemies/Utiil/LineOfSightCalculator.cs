using UnityEngine;

public class LineOfSightCalculator : MonoBehaviour
{
    private Transform player;

    [SerializeField] private LayerMask losObstacles;
    void Start()
    {
        player = PlayerLocator.Player;
    }

    public bool HasLineOfSight
    {
        get
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out RaycastHit hit, distanceToPlayer, losObstacles))
            {
                return false;
            }

            return true;
        }
    }
}
