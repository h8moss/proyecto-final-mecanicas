using System.Collections.Generic;
using UnityEngine;

public class TornadoFuego : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 5;
    public float damageRate = 1f;
    public float radius = 3f;

    private Dictionary<EnemyHealthControler, float> enemyTimers = new Dictionary<EnemyHealthControler, float>();
    private Dictionary<BossHealthManager, float> bossTimers = new Dictionary<BossHealthManager, float>();
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        HashSet<EnemyHealthControler> currentEnemies = new HashSet<EnemyHealthControler>();
        HashSet<BossHealthManager> currentBosses = new HashSet<BossHealthManager>();
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
            var boss = hit.GetComponent<BossHealthManager>();
            if (boss != null)
            {
                currentBosses.Add(boss);
                if (!bossTimers.ContainsKey(boss)) bossTimers[boss] = 0f;

                bossTimers[boss] += Time.deltaTime;
                if (bossTimers[boss] >= damageRate)
                {
                    boss.TakeDamage(damage); 
                    bossTimers[boss] = 0f;
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

        List<BossHealthManager> bossesToRemove = new List<BossHealthManager>();
        foreach (var kvp in bossTimers)
        {
            if (!currentBosses.Contains(kvp.Key)) bossesToRemove.Add(kvp.Key);
        }
        foreach (var b in bossesToRemove) bossTimers.Remove(b);
    }
}
