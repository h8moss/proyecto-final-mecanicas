using UnityEngine;

public class MainPlayerNormalAttack : MonoBehaviour
{
    public Animator animator;
    public float comboResetTime = 0.6f; 

    private int comboStep = 0; 
    private float lastClickTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAttackInput();
        }
    }

    void HandleAttackInput()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        // Si tardó mucho, resetear el combo
        if (timeSinceLastClick > comboResetTime)
            comboStep = 0;

        comboStep++;
        lastClickTime = Time.time;

        if (comboStep == 1)
        {
            animator.SetTrigger("attack1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("attack2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("attack3");
            comboStep = 0; 
        }
    }
}
