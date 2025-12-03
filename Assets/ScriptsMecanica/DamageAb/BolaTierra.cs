using UnityEngine;

public class BolaTierra : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 35;
    public float impactRadius = 3f;
    public float knockbackForce = 5f;

    [Header("Effects")]
    public GameObject impactEffectPrefab;

    private bool hasImpacted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasImpacted) return;

        if (other.CompareTag("Enemy"))
        {
            hasImpacted = true;

            // Daño y knockback
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, impactRadius);
            foreach (var hit in hitColliders)
            {
                var enemy = hit.GetComponent<EnemyHealthControler>();
                if (enemy != null)
                {
                    enemy.DealDamage(damage);

                    if (hit.attachedRigidbody != null)
                    {
                        Vector3 dir = hit.transform.position - transform.position;
                        dir.y = 0;
                        hit.attachedRigidbody.AddForce(dir.normalized * knockbackForce, ForceMode.Impulse);
                    }
                }
            }

            // Efecto visual
            if (impactEffectPrefab != null)
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
