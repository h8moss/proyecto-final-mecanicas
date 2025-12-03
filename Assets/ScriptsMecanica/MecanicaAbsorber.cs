using UnityEngine;

public class MecanicaAbsorber : MonoBehaviour
{
    public AbilityBase[] slots = new AbilityBase[3];
    public PlayerHealth vidaJugador;

    private Alma alma;
    private bool almaExists = false;

    void Update()
    {
        int slot = SlotDisponible();
        if (almaExists)
        {
            if (Input.GetKeyDown(KeyCode.F) && slot != -1)
            {
                AbsorberHabilidad(alma);
                alma.gameObject.SetActive(false);
                almaExists = false;
            }
            else if (Input.GetKeyDown(KeyCode.C) && vidaJugador.Health != vidaJugador.MaxHealth)
            {
                vidaJugador.Heal(Random.Range(15, 26));
                alma.gameObject.SetActive(false);
                almaExists = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Alma"))
        {
            alma = other.GetComponent<Alma>();
            almaExists = true;
        }
    }

    private void AbsorberHabilidad(Alma alma)
    {
        int slot = SlotDisponible();
        if (slot == -1) return;

        AbilityBase habilidad = Instantiate(alma.habilidadPrefab, transform);

        slots[slot] = habilidad;
    }

    private int SlotDisponible()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                return i;
        }
        return -1;
    }
}
