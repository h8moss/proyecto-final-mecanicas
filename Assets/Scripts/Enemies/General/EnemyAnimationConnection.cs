using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealthControler))]
[RequireComponent(typeof(BaseEnemyAttack))]
public class EnemyAnimationConnection : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private EnemyHealthControler health;
    private BaseEnemyAttack attack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealthControler>();
        attack = GetComponent<BaseEnemyAttack>();

        health.onDeath += Death;
        attack.onAttack += Attack;
    }

    void Update()
    {
        Vector3 speedDirection = agent.velocity.normalized;
        animator.SetFloat("MovementX", Vector3.Dot(speedDirection, transform.right));
        animator.SetFloat("MovementY", Vector3.Dot(speedDirection, transform.forward));
    }

    void Death()
    {
        animator.SetTrigger("Death");
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
