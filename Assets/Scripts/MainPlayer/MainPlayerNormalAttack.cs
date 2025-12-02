using System.Collections;
using UnityEngine;

public class MainPlayerNormalAttack : MonoBehaviour
{
    public Animator animator;
    public float comboResetTime = 0.6f; 

    [SerializeField] GameObject combo1Hitbox;
    [SerializeField] GameObject combo2Hitbox;
    [SerializeField] GameObject combo3Hitbox;

    [SerializeField] int combo1Damage;
    [SerializeField] int combo2Damage;
    [SerializeField] int combo3Damage;

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

        // Si tard� mucho, resetear el combo
        if (timeSinceLastClick > comboResetTime)
            comboStep = 0;

        comboStep++;
        lastClickTime = Time.time;

        if (comboStep == 1)
        {
            animator.SetTrigger("attack1");
            StartCoroutine(HitCombo(1));
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("attack2");
            StartCoroutine(HitCombo(2));
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("attack3");
            StartCoroutine(HitCombo(3));
            comboStep = 0; 
        }
    }

    IEnumerator HitCombo(int number)
    {
        number -= 1;
        GameObject[] combos = new GameObject[]
        {
            combo1Hitbox, combo2Hitbox, combo3Hitbox
        };

        for (int i=0; i<3; i++)
        {
            combos[i].SetActive(number == i);
        }
        yield return new WaitForSeconds(comboResetTime);
        combos[number].SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent<EnemyHealthControler>(out var eh))
        {
            var dmg = 0;
            switch (comboStep)
            {
                case 1:
                    dmg = combo1Damage;
                    break;
                case 2:
                    dmg = combo2Damage;
                    break;
                case 3:
                    dmg = combo3Damage;
                    break;
            }

            eh.DealDamage(dmg);
        }
    }
}
