using UnityEngine;

public class TornadoFuego : MonoBehaviour
{
    public int damage = 5;
    public float damageRate = 1f;

    private float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        if (timer >= damageRate)
        {
            var enemy = other.GetComponent<EnemyHealthControler>();
            if (enemy != null)
                enemy.DealDamage(damage);

            timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0f;
    }
}
