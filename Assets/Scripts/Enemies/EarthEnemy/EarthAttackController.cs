using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
[RequireComponent(typeof(EnemyHealthControler))]
[RequireComponent(typeof(PooledObjectHelper))]
public class EarthAttackController : BaseEnemyAttack
{
    [SerializeField] float fireCooldown;
    [SerializeField] Transform bulletSpawnPosition;

    ObjectPooler objectPooler;

    void Start()
    {
        health = GetComponent<EnemyHealthControler>();
        objectPooler = GetComponent<ObjectPooler>();
        GetComponent<PooledObjectHelper>().onReset += Reset;
        Reset();
    }

    void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireCooldown);
            if (!CanAttack)
            {
                continue;
            }

            onAttack?.Invoke();
            GameObject bullet = objectPooler.GetObject();
            bullet.transform.position = bulletSpawnPosition.position;
            if (bullet == null) 
            {
                continue;
            }

            
            bullet.GetComponent<EarthProjectileController>().SetTarget(PlayerLocator.Player.position);
        }
    }
}

