using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PooledObjectHelper))]
public class LighningAttackController : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject lightningObject;
    [SerializeField] private float trackingTime;
    [SerializeField] private float stillTime;
    [SerializeField] private float attackDuration;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float targetHeight;


    private Coroutine attackRoutine;
    private LighningAttackState state;

    void Start()
    {
        GetComponent<PooledObjectHelper>().onReset += Reset;
        Reset();
    }

    void Update()
    {
        if (state == LighningAttackState.Tracking)
        {
            Vector3 target = new(PlayerLocator.Player.position.x, PlayerLocator.Player.position.y + targetHeight, PlayerLocator.Player.position.z);
            var initialPos = new Vector3(
                transform.position.x,
                PlayerLocator.Player.position.y + targetHeight,
                transform.position.z
            );
            transform.position = Vector3.Lerp(initialPos, target, smoothSpeed * Time.deltaTime);

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 50, ground))
            {
                transform.position += Vector3.down * (hit.distance - 0.5f);
            }
        }
    }


    void Reset()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        attackRoutine = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        targetObject.SetActive(true);
        lightningObject.SetActive(false);
        state = LighningAttackState.Tracking;
        yield return new WaitForSeconds(trackingTime);
        state = LighningAttackState.Still;
        yield return new WaitForSeconds(stillTime);
        state = LighningAttackState.Attacking;
        targetObject.SetActive(false);
        lightningObject.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        gameObject.SetActive(false);
    }
}
