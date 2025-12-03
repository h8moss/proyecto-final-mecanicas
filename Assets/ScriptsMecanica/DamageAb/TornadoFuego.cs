using System.Collections.Generic;
using UnityEngine;

public class TornadoFuego : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 5;
    public float damageRate = 1f;
    public float radius = 3f;

    private Dictionary<EnemyHealthControler, float> enemyTimers = new Dictionary<EnemyHealthControler, float>();

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        HashSet<EnemyHealthControler> currentEnemies = new HashSet<EnemyHealthControler>();

        foreach (var hit in hitColliders)
        {
            var enemy = hit.GetComponent<EnemyHealthControler>();
            if (enemy != null)
            {
                currentEnemies.Add(enemy);

                if (!enemyTimers.ContainsKey(enemy))
                    enemyTimers[enemy] = 0f;

                enemyTimers[enemy] += Time.deltaTime;

                if (enemyTimers[enemy] >= damageRate)
                {
                    enemy.DealDamage(damage);
                    enemyTimers[enemy] = 0f;
                }
            }
        }

        List<EnemyHealthControler> toRemove = new List<EnemyHealthControler>();
        foreach (var kvp in enemyTimers)
        {
            if (!currentEnemies.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }
        foreach (var e in toRemove)
            enemyTimers.Remove(e);
    }
}
