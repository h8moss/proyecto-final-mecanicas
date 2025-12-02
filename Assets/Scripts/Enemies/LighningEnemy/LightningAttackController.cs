using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyHealthControler))]
[RequireComponent(typeof(ObjectPooler))]
[RequireComponent(typeof(PooledObjectHelper))]
public class LightningAttackController : BaseEnemyAttack
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
            if (bullet == null) 
            {
                continue;
            }
            bullet.transform.position = bulletSpawnPosition.position;
        }
    }
}
