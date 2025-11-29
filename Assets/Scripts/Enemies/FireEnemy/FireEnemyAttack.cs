using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ObjectPooler))]
public class FireEnemyAttack : BaseEnemyAttack
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

                // Get current player state
                Vector3 currentPlayerPos = PlayerLocator.Player.position;
                float currentTime = Time.time;

                // Calculate player velocity
                float deltaTime = currentTime - previousTime;
                Vector3 playerVelocity = (currentPlayerPos - previousPlayerPos) / deltaTime;

                // Calculate bullet travel time
                float distanceToPlayer = Vector3.Distance(bulletSpawnPosition.position, currentPlayerPos);
                float bulletSpeed = bullet.GetComponent<EnemyBulletController>().Speed;
                float travelTime = distanceToPlayer / bulletSpeed;

                // Predict where player will be
                Vector3 predictedPosition = currentPlayerPos + (playerVelocity * travelTime);

                // Aim at predicted position
                Vector3 lookDirection = predictedPosition - bulletSpawnPosition.position;
                lookDirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                bullet.transform.SetPositionAndRotation(bulletSpawnPosition.position, rotation);

                // Update tracking
                previousPlayerPos = currentPlayerPos;
                previousTime = currentTime;

                yield return new WaitForSeconds(timeBetweenFires);
            }
        }
    }
}
