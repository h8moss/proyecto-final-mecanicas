using System.Collections;
using UnityEngine;

public class AireDamage : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 25;
    public float impactoRadius = 5f;
    public float impactoDelay = 0.3f;

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

        Destroy(gameObject, 2);
    }

}
