using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] LayerMask collisionLayers;
    [SerializeField] bool deactivateSelf = true;

    void OnTriggerEnter(Collider other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth ph))
            {
                ph.DealDamage(damage);
            }

            if (deactivateSelf) gameObject.SetActive(false);
        }
    }
}
