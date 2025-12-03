using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 15;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            PlayerHealth playerHP = other.GetComponent<PlayerHealth>();

            if (playerHP != null)
            {
                playerHP.DealDamage(damage);
            }

            Destroy(gameObject); 
        }
        else if (other.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
}