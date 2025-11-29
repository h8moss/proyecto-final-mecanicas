using UnityEngine;


[RequireComponent(typeof(EnemyHealthControler))]
public class BaseEnemyAttack : MonoBehaviour
{
    public delegate void OnAttack();
    public OnAttack onAttack;

    protected bool CanAttack { get => !health.IsDead; }

    protected EnemyHealthControler health;
}
