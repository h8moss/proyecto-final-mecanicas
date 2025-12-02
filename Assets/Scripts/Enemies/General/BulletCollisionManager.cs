using UnityEngine;

public class BulletCollisionManager : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] LayerMask collisionLayers;

    void OnTriggerEnter(Collider other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            gameObject.SetActive(false);

            if (other.gameObject.TryGetComponent(out PlayerHealth ph))
            {
                ph.DealDamage(damage);
            }
        }
    }
}
