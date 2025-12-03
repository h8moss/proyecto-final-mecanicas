using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineOfSightCalculator))]
[RequireComponent(typeof(PooledObjectHelper))]
[RequireComponent(typeof(EnemyHealthControler))]
public class FireEnemyMovement : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float preferredDistance = 8f;
    [SerializeField] private float distanceRandomness = 2f;

    [Header("Movement Settings")]
    [SerializeField] private float circleSpeed = 2f;

    [Header("Separation Settings")]
    [SerializeField] private float separationRadius = 3f;
    [SerializeField] private float separationStrength = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private float currentPreferredDistance;

    private Transform player;
    private NavMeshAgent agent;
    private LineOfSightCalculator los;
    private EnemyHealthControler health;
    private bool initialized;

    void Start()
    {
        player = PlayerLocator.Player;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        los = GetComponent<LineOfSightCalculator>();
        GetComponent<PooledObjectHelper>().onReset += Reset;

        health = GetComponent<EnemyHealthControler>();

        Reset();
    }

    void Reset() {
        initialized = false;
        currentPreferredDistance = preferredDistance + Random.Range(-distanceRandomness, distanceRandomness);
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null; // Wait one frame
        initialized = true;
    }



    void Update()
    {
        if (health.IsDead || !initialized)
        {
            agent.destination = transform.position;
            return;
        }
        // Look at player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        Vector3 target;
        Vector3 separation = CalculateSeparation();
        if (separation != Vector3.zero)
        {
            target = separation;
        } else
        {
            if (los.HasLineOfSight)
            {
                target = CalculateCirclePosition();
            }
            else
            {
                target = player.position;
            }
        }

        agent.destination = target;
    }

    Vector3 CalculateCirclePosition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
    
        if (Mathf.Abs(distanceToPlayer - currentPreferredDistance) < 1f)
        {
            //Vector3 rightDirection = Vector3.Cross(Vector3.up, directionToPlayer);
            Vector3 rightDirection = transform.right;

            Vector3 targetPosition = transform.position + rightDirection * circleSpeed;
            return targetPosition;
        }
        else
        {
            Vector3 targetPosition = player.position + (-directionToPlayer * currentPreferredDistance);
            return targetPosition;
        }
    }

    Vector3 CalculateSeparation()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);
        Vector3 separationForce = Vector3.zero;

        foreach (Collider other in nearbyEnemies)
        {
            if (other.transform == transform) continue;

            Vector3 awayFromOther = transform.position - other.transform.position;
            float distance = awayFromOther.magnitude;

            if (distance > 0)
            {
                // Stronger force when closer
                float strength = (1f - (distance / separationRadius)) * separationStrength;
                separationForce += awayFromOther.normalized * strength;
            }
        }

        return separationForce;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Draw preferred distance circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, currentPreferredDistance);

        // Draw separation radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        // Draw LOS ray
        Gizmos.color = los.HasLineOfSight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up, player.position + Vector3.up);
        
        // Draw target position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(agent.destination, Vector3.one*0.5f);
    }

}

