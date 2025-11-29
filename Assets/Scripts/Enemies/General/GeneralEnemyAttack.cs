using System.Collections;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class GeneralEnemyAttack : BaseEnemyAttack
{
    [SerializeField] float fireCooldown;
    [SerializeField] float timeBetweenFires;
    [SerializeField] int fireCount;
    [SerializeField] Transform bulletSpawnPosition;

    ObjectPooler objectPooler;

    void Start()
    {
        health = GetComponent<EnemyHealthControler>();
        objectPooler = GetComponent<ObjectPooler>();
        Reset();
    }

    void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }
    IEnumerator AttackRoutine()
    {
        Vector3 previousPlayerPos = PlayerLocator.Player.position;
        float previousTime = Time.time;
    
        while (true)
        {
            yield return new WaitForSeconds(fireCooldown);
            if (!CanAttack)
            {
                continue;
            }

            for (int _ = 0; _ < fireCount; _++)
            {
                if (!CanAttack) break;
                onAttack?.Invoke();
                GameObject bullet = objectPooler.GetObject();
                if (bullet == null) 
                {
                    yield return new WaitForSeconds(timeBetweenFires);
                    continue;
                }

                // Aim at player
                Vector3 lookDirection = PlayerLocator.Player.position - bulletSpawnPosition.position;
                lookDirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                bullet.transform.SetPositionAndRotation(bulletSpawnPosition.position, rotation);

                if (bullet.TryGetComponent<EarthProjectileController>(out var earth))
                {
                    earth.SetTarget(PlayerLocator.Player.position);
                }
            }
        }
    }
}
