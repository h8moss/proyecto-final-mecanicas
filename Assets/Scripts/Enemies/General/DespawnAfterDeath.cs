using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FireEnemyMovement))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealthControler))]
[RequireComponent(typeof(PooledObjectHelper))]
public class DespawnAfterDeath : MonoBehaviour
{
    [SerializeField] GameObject[] instantDisable;
    [SerializeField] float disabledTimeout;
    [SerializeField] Collider colliderToDisable;
    [SerializeField] GameObject soul;

    EnemyHealthControler health;
    PooledObjectHelper helper;
    FireEnemyMovement movement;
    NavMeshAgent agent;

    void Start()
    {
        movement = GetComponent<FireEnemyMovement>();
        agent = GetComponent<NavMeshAgent>();

        health = GetComponent<EnemyHealthControler>();
        health.onDeath += Death;

        helper = GetComponent<PooledObjectHelper>();
        helper.onReset += Reset;
        Reset();
    }

    void Death()
    {
        StartCoroutine(Disable());
    }

    void Reset()
    {
        foreach (var go in instantDisable)
        {
            go.SetActive(true);
        }
        movement.enabled = true;
        colliderToDisable.enabled = true;
        agent.enabled = true;
        soul.SetActive(false);
    }

    IEnumerator Disable()
    {
        foreach (var go in instantDisable)
        {
            go.SetActive(false);
        }
        movement.enabled = false;
        agent.enabled = false;
        colliderToDisable.enabled = false;
        soul.SetActive(true);

        yield return new WaitForSeconds(disabledTimeout);

        gameObject.SetActive(false);

    }
}
