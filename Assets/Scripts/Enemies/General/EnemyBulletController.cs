using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PooledObjectHelper))]
public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask[] collisionLayers;
    [SerializeField] private float lifeTime;
    [SerializeField] private bool shrinkWithLife;
    [SerializeField] private bool trackPlayer;

    public float Speed { get => speed; }

    PooledObjectHelper poh;
    Coroutine lifetimeCoroutine;

    float lifetimeStart;
    Vector3 initialScale;

    void Start()
    {
        poh = GetComponent<PooledObjectHelper>();
        poh.onReset += Reset;
        initialScale = transform.localScale;
        Reset();
    }

    void Reset()
    {
        transform.localScale = initialScale;
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }
        lifetimeCoroutine = StartCoroutine(LifetimeCounter());
    }

    void Update()
    {
        if (shrinkWithLife)
        {
            float timeAlive = Time.time - lifetimeStart;
            transform.localScale = initialScale * (1 - (timeAlive / lifeTime));
        }
        if (trackPlayer)
        {
            Vector3 lookDirection = PlayerLocator.Player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: Damage player

        if (collisionLayers.Contains(other.gameObject.layer)) {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    IEnumerator LifetimeCounter()
    {
        lifetimeStart = Time.time;
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
