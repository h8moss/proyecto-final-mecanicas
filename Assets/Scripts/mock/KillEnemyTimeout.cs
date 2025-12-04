using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyHealthControler))]
public class KillEnemyTimeout : MonoBehaviour
{
    [SerializeField] float timeout;
    void Start()
    {
        StartCoroutine(KillEnemy());
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(timeout);
        var health = GetComponent<EnemyHealthControler>();
        health.DealDamage(health.Health + 1);
    }
}
