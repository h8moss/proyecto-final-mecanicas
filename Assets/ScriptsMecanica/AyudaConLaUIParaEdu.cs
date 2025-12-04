using UnityEngine;

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
