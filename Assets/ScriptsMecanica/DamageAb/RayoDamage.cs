using System.Collections;
using UnityEngine;

public class RayoDamage : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 20;
    public float impactoRadius = 3f;
    public float impactoDelay = 0.5f;


    private void Start()
    {
        StartCoroutine(ImpactCoroutine());
    }

    private IEnumerator ImpactCoroutine()
    {
        yield return new WaitForSeconds(impactoDelay);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, impactoRadius);
        foreach (var hit in hitColliders)
        {
            var enemy = hit.GetComponent<EnemyHealthControler>();
            if (enemy != null)
                enemy.DealDamage(damage);
        }

        Destroy(gameObject);
    }
}
