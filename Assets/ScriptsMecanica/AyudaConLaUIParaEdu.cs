using UnityEngine;
using UnityEngine.UI;

public class AyudaConLaUIParaEdu : MonoBehaviour
{
    private MecanicaAbsorber absorbido;

    public string[] nombresAbilities;
    [SerializeField] private AbilityBase[] abilities;

    // 3 slots separados, cada uno con 5 imágenes
    public Image[] slot1; // Fuego, Aire, Tierra, Hielo, Rayo
    public Image[] slot2;
    public Image[] slot3;

    private Image[][] slots; // Solo para uso interno

    void Start()
    {
        absorbido = FindAnyObjectByType<MecanicaAbsorber>();

        // Juntamos los 3 slots en un arreglo interno (solo código)
        slots = new Image[3][];
        slots[0] = slot1;
        slots[1] = slot2;
        slots[2] = slot3;
    }

    void Update()
    {
        if (absorbido == null)
            return;

        abilities = absorbido.slots;

        if (nombresAbilities == null || nombresAbilities.Length != abilities.Length)
        {
            nombresAbilities = new string[abilities.Length];
        }

        // Desactivar todas las imágenes
        for (int s = 0; s < slots.Length; s++)
        {
            for (int j = 0; j < slots[s].Length; j++)
            {
                slots[s][j].gameObject.SetActive(false);
            }
        }

        // Llenar los slots disponibles con las habilidades activas
        int slotIndex = 0;

        for (int i = 0; i < abilities.Length; i++)
        {
            string nombre = abilities[i] != null ? abilities[i].abilityName : "vacío";
            nombresAbilities[i] = nombre;

            if (nombre != "vacío" && slotIndex < slots.Length)
            {
                Image[] currentSlot = slots[slotIndex];

                switch (nombre)
                {
                    case "Fuego": currentSlot[0].gameObject.SetActive(true); break;
                    case "Aire": currentSlot[1].gameObject.SetActive(true); break;
                    case "Tierra": currentSlot[2].gameObject.SetActive(true); break;
                    case "Hielo": currentSlot[3].gameObject.SetActive(true); break;
                    case "Rayo": currentSlot[4].gameObject.SetActive(true); break;
                }

                slotIndex++; // Pasamos al siguiente slot libre
            }
        }
    }
}

/*
using UnityEngine;
using UnityEngine.UI;

public class AyudaConLaUIParaEdu : MonoBehaviour
{
    private MecanicaAbsorber absorbido;

    public string[] nombresAbilities;
    [SerializeField] private AbilityBase[] abilities;

    // AGREGADO: imágenes UI para cada habilidad
    public Image[] imagenesUI;

    void Start()
    {
        absorbido = FindAnyObjectByType<MecanicaAbsorber>();
    }

    void Update()
    {
        abilities = absorbido.slots;

        // Asegurar tamaño correcto
        if (nombresAbilities == null || nombresAbilities.Length != abilities.Length)
        {
            nombresAbilities = new string[abilities.Length];
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            // Sacar el nombre
            if (abilities[i] != null)
            {
                nombresAbilities[i] = abilities[i].abilityName;
            }
            else
            {
                nombresAbilities[i] = "vacío";
            }

            // ACTIVAR / DESACTIVAR IMÁGENES UI
            if (imagenesUI != null && i < imagenesUI.Length)
            {
                if (nombresAbilities[i] != "vacío")
                {
                    imagenesUI[i].gameObject.SetActive(true);
                }
                else
                {
                    imagenesUI[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
*/


/*using UnityEngine;

public class AyudaConLaUIParaEdu : MonoBehaviour
{
    private MecanicaAbsorber absorbido;

    public string[] nombresAbilities;
    [SerializeField] private AbilityBase[] abilities;

    void Start()
    {
        absorbido = GetComponentInParent<MecanicaAbsorber>();
    }

    void Update()
    {
        abilities = absorbido.slots;

        if (nombresAbilities == null || nombresAbilities.Length != abilities.Length)
        {
            nombresAbilities = new string[abilities.Length];
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                nombresAbilities[i] = abilities[i].abilityName;
            }
            else
            {
                nombresAbilities[i] = "vacío";
            }
        }
    }
}
*/