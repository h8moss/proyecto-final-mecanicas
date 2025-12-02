using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyHealthControler))]
[RequireComponent(typeof(PooledObjectHelper))]
public class DespawnAfterDeath : MonoBehaviour
{
    [SerializeField] GameObject[] instantDisable;
    [SerializeField] float disabledTimeout;

    EnemyHealthControler health;
    PooledObjectHelper helper;

    void Start()
    {
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
    }

    IEnumerator Disable()
    {
        foreach (var go in instantDisable)
        {
            go.SetActive(false);
        }
        yield return new WaitForSeconds(disabledTimeout);
        gameObject.SetActive(false);
    }
}
