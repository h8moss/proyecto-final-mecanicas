using UnityEngine;

public class MecanicaAbsorber : MonoBehaviour
{
    public AbilityBase[] slots = new AbilityBase[3];   // << AHORA habilidades reales
    public PlayerHealth vidaJugador;

    private Alma alma;
    private bool almaExists = false;

    void Update()
    {
        if (almaExists)
        {
            if (Input.GetKeyDown(KeyCode.F)) // absorber habilidad
            {
                AbsorberHabilidad(alma);
                alma.gameObject.SetActive(false);
                almaExists = false;
            }
            else if (Input.GetKeyDown(KeyCode.C)) // curar
            {
                vidaJugador.Heal(Random.Range(15, 26));
                alma.gameObject.SetActive(false);
                almaExists = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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

        // Instanciamos la habilidad que viene en el alma
        AbilityBase habilidad = Instantiate(alma.habilidadPrefab, transform);

        slots[slot] = habilidad;
        Debug.Log("Absorbiste habilidad: " + habilidad.name);
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
